using Ktru.infrastructure;
using Ktru.model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using ZakupkiUtils.infrastructure;

namespace Ktru.operation
{
    class ProgressZakupkiFile
    {
        public ZakupkiFile ZF { get; set; }
        public long Progress { get; set; }
        public string TextInfo { get; set; }
    }

    class OperationLayer
    {
        public OperationLayer(
            IZakupkiFileService zakupkiFileService,
            IZakupkiSettings zakupkiSettings,
            IXlsxOperation xlsxOperation,
            IDomainModel model)
        {
            fileService = zakupkiFileService;
            settings = zakupkiSettings;
            xlsx = xlsxOperation;
            domainModel = model;
        }

        public void CheckKtruRelevance()
        {
            domainModel.IsKtruModified = false;
            IEnumerable<ZakupkiFile> zakupkiFiles = fileService.GetFiles(settings.GetKtruDir());
            IEnumerable<ZakupkiFile> localFiles = domainModel.GetLocalFiles(settings.GetLocalKtruDir());
            domainModel.IsKtruModified = !domainModel.EqualsWithoutParent(zakupkiFiles, localFiles);
        }

        public async Task UpdateKtru(Action<ProgressZakupkiFile> progress, Action<string> result)
        {
            string localKtruDir = settings.CreateLocalKtruDirIfNeed(out string error);
            if (error != string.Empty)
            {
                result(error);
                return;
            }
            IEnumerable<ZakupkiFile> zakupkiFiles = fileService.GetFiles(settings.GetKtruDir());
            foreach (var zakupkiFile in zakupkiFiles)
            {
                await DownloadFile(localKtruDir, zakupkiFile, x =>
                {
                    var p = new ProgressZakupkiFile
                    {
                        ZF = zakupkiFile,
                        Progress = x,
                        TextInfo = "Удалённая загрузка файла"
                    };
                    progress(p);
                },
                e =>
                {
                    error = e;
                });
                if (error != string.Empty)
                {
                    break;
                }
            }
            if (error == string.Empty)
            {
                bool found;
                IEnumerable<ZakupkiFile> localFiles = domainModel.GetLocalFiles(localKtruDir);
                foreach(var localFile in localFiles)
                {
                    found = false;
                    foreach(var zakupkiFile in zakupkiFiles)
                    {
                        if (localFile.EqualsWithoutParent(zakupkiFile))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        File.Delete(localFile.FullPath());
                    }
                }
            }
            result(error);
        }

        public async Task BuildKtruFile(
            string resultFile,
            Action<ProgressZakupkiFile> progress,
            Action<string> result,
            bool onlyActual,
            bool needClear)
        {
            string archDir = settings.PrepareLocalKtruArchiveDir(out string error);
            if (error != string.Empty)
            {
                result(error);
                return;
            }
            try
            {
                if (File.Exists(resultFile))
                {
                    File.Delete(resultFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                result(e.Message);
                return;
            }

            await ExtractLocalKtruFiles(archDir, progress, e => { error = e; });
            if(error != string.Empty)
            {
                result(error);
                return;
            }
            await LoadKtruFilesIntoXlsx(archDir, resultFile, onlyActual, progress, e => { error = e; });
            if (error != string.Empty)
            {
                result(error);
                return;
            }
            if (needClear)
                settings.ClearLocalKtruArchiveDir(out error);
            result(error);
        }

        private async Task DownloadFile(
            string targetDir,
            ZakupkiFile file,
            Action<long> progress,
            Action<string> error)
        {
            string localFile = targetDir + '\\' + file.Name;
            var f = domainModel.GetLocalFile(localFile, out bool ok);
            if (ok && file.EqualsWithoutParent(f))
            {
                return;
            }
            await fileService.DownloadFile(file, localFile, progress, error);
        }

        private async Task ExtractLocalKtruFiles(
            string archDir,
            Action<ProgressZakupkiFile> progress,
            Action<string> error)
        {
            try
            {
                IEnumerable<ZakupkiFile> localFiles = domainModel.GetLocalFiles(settings.GetLocalKtruDir());
                foreach (ZakupkiFile localFile in localFiles)
                {
                    await Task.Run(() => ZipFile.ExtractToDirectory(localFile.FullPath(), archDir));
                    progress(new ProgressZakupkiFile
                    {
                        ZF = localFile,
                        Progress = localFile.Size,
                        TextInfo = "Распаковка файла"
                    });
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                error(e.Message);
                return;
            }
            error(string.Empty);
        }

        private async Task LoadKtruFilesIntoXlsx(
            string archDir,
            string resultFile,
            bool onlyActual,
            Action<ProgressZakupkiFile> progress,
            Action<string> error)
        {
            try
            {
                HashSet<KtruItem> ktrus = new HashSet<KtruItem>();
                IEnumerable<string> localXmlFiles = Directory.EnumerateFiles(archDir, "*.xml");
                foreach (string localXml in localXmlFiles)
                {
                    var f = domainModel.GetLocalFile(localXml, out bool ok);
                    Trace.Assert(ok);
                    progress(new ProgressZakupkiFile
                    {
                        ZF = f,
                        Progress = f.Size,
                        TextInfo = "Разбор данных файла"
                    });
                    await LoadXmlAndParse(localXml, y =>
                    {
                        foreach (var k in y)
                        {
                            ktrus.Remove(k);
                            if (onlyActual)
                            {
                                if (!k.Actual)
                                    continue;
                            }
                            ktrus.Add(k);
                        }
                    });
                }
                var sortedKtrus = ktrus.ToList().OrderBy(x => x.Code).ThenBy(y => y.Version);
                xlsx.SaveKtruFile(resultFile, sortedKtrus);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                error(e.Message);
                return;
            }
            error(string.Empty);
        }

        private static async Task LoadXmlAndParse(string localXml, Action<List<KtruItem>> callback)
        {
            BlockingCollection<KtruItem> result = new BlockingCollection<KtruItem>();
            await Task.Run(() =>
            {
                using (XmlTextReader reader = new XmlTextReader(localXml))
                {
                    var sm = new KtruStateMachineController(reader);
                    sm.Run();
                    foreach (var k in sm.GetKtrus())
                        result.Add(k);
                }
            });
            List<KtruItem> r = new List<KtruItem>();
            foreach (var k in result)
                r.Add(k);
            callback(r);
        }

        private void AddOrReplace(List<KtruItem> ktrus, KtruItem k)
        {
            bool found = false;
            foreach (var ktru in ktrus)
            {
                if (ktru.Code == k.Code
                    && ktru.Version == k.Version
                    && ktru.Name == k.Name)
                {
                    ktru.Actual = k.Actual;
                    ktru.StartDate = k.StartDate;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                ktrus.Add(k);
            }
        }

        private readonly IZakupkiFileService fileService;
        private readonly IZakupkiSettings settings;
        private readonly IXlsxOperation xlsx;
        private readonly IDomainModel domainModel;

    }
}
