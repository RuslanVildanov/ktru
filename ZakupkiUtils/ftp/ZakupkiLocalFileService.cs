using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ZakupkiUtils.infrastructure;

namespace ZakupkiUtils.ftp
{
    class ZakupkiLocalFileService : IZakupkiLocalFileService
    {
        public ZakupkiLocalFileService(IZakupkiSettings zakupkiSettings)
        {
            settings = zakupkiSettings;
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

        public bool EqualsWithoutParent(IEnumerable<ZakupkiFile> f1, IEnumerable<ZakupkiFile> f2)
        {
            bool equals = true;
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
                    equals = false;
                    break;
                }
            }
            return equals;
        }

        private readonly IZakupkiSettings settings;
    }
}
