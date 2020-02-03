using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Ktru.infrastructure;
using ZakupkiUtils.infrastructure;

namespace Ktru.repository
{
    class DomainRepository : IDomainRepository
    {
        public DomainRepository(IZakupkiSettings zakupkiSettings)
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
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

        private readonly IZakupkiSettings settings;
    }
}
