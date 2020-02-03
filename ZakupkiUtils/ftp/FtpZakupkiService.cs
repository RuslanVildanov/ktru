using ZakupkiUtils.infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZakupkiUtils.ftp
{
    public class FtpZakupkiService : IZakupkiFileService
    {
        public IEnumerable<ZakupkiFile> GetFiles(string zakupki_dir)
        {
            var result = new List<ZakupkiFile>();
            var output = FtpZakupkiServiceStatic.GetFiles(zakupki_dir);
            foreach(var item in output)
            {
                result.Add(new ZakupkiFile(item.ParentDir, item.Name, item.Modified, item.Size, item.IsFile));
            }
            return result;
        }

        public async Task DownloadFile(
            ZakupkiFile file,
            string targetLocalFile,
            Action<long> progress,
            Action<string> error)
        {
            ZakupkiFile f = new ZakupkiFile(
                file.ParentDir,
                file.Name,
                file.Modified,
                file.Size,
                file.IsFile);
            await FtpZakupkiServiceStatic.DownloadFile(f, targetLocalFile, progress, error);
        }

    }
}
