using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ZakupkiUtils.infrastructure;

namespace ZakupkiUtils.ftp
{
    class ZakupkiLocalFileService : IZakupkiLocalFileService
    {
        public ZakupkiLocalFileService(IZakupkiSettings zakupkiSettings)
        {
            _settings = zakupkiSettings;
        }

        public IEnumerable<ZakupkiFile> GetLocalFiles(string localDir)
        {
            IList<ZakupkiFile> result = new List<ZakupkiFile>();
            string[] files = new string[0];
            try
            {
                files = Directory.GetFiles(localDir);
            }
            catch(Exception)
            {
                // контролируемое исключение, на верхнем уровне не интересны
                // причины поему вернётся пустой список локальных файлов
            }
            foreach (var filePath in files)
            {
                var f = GetLocalFile(filePath, out bool ok);
                Trace.Assert(ok);
                result.Add(f);
            }
            return result;
        }

        public ZakupkiFile GetLocalFile(string localFile, out bool ok)
        {
            if (!File.Exists(localFile))
            {
                ok = false;
                return null;
            }
            var fi = new FileInfo(localFile);
            ok = true;
            return new ZakupkiFile(fi.DirectoryName, fi.Name, fi.LastWriteTime, fi.Length, true);
        }

        public void RemoveNotFoundLocalFiles(string localDir, IEnumerable<ZakupkiFile> notFoundIn, out string error)
        {
            error = string.Empty;
            bool found;
            IEnumerable<ZakupkiFile> localFiles = GetLocalFiles(localDir);
            foreach (var localFile in localFiles)
            {
                found = false;
                foreach (var zakupkiFile in notFoundIn)
                {
                    if (localFile.EqualsWithoutParent(zakupkiFile))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    try
                    {
                        File.Delete(localFile.FullPath());
                    }
                    catch (Exception e)
                    {
                        error = e.Message;
                    }
                }
            }
        }

        public bool EqualsWithoutParent(IEnumerable<ZakupkiFile> f1, IEnumerable<ZakupkiFile> f2)
        {
            if (f1.Count() != f2.Count())
            {
                return false;
            }
            foreach (ZakupkiFile zakupkiFile in f1)
            {
                bool foundEquals = false;
                foreach (ZakupkiFile localFile in f2)
                {
                    if (!zakupkiFile.IsFile)
                    {
                        continue;
                    }
                    if (zakupkiFile.EqualsWithoutParent(localFile))
                    {
                        foundEquals = true;
                        break;
                    }
                }
                if (!foundEquals)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task ExtractLocalZipFiles(
            string zipFilesDir,
            string targetDir,
            Action<string> progress,
            Action<string> error)
        {
            try
            {
                IEnumerable<ZakupkiFile> localFiles = GetLocalFiles(zipFilesDir);
                bool hasError = false;
                foreach (ZakupkiFile localFile in localFiles)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            ZipFile.ExtractToDirectory(localFile.FullPath(), targetDir);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                            error(e.Message);
                            hasError = true;
                            return;
                        }
                    });
                    if (hasError)
                    {
                        break;
                    }
                    progress(localFile.FullPath());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                error(e.Message);
                return;
            }
        }

        private readonly IZakupkiSettings _settings;
    }
}
