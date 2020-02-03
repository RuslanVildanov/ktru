using Okpd2.infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Okpd2.ftp
{
    class FtpZakupkiService : IZakupkiFileService
    {
        public IEnumerable<ZakupkiFile> GetFiles(string zakupki_dir)
        {
            var result = new List<ZakupkiFile>();
            var output = ZakupkiUtils.FtpZakupkiService.GetFiles(zakupki_dir);
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
            ZakupkiUtils.ZakupkiFile f = new ZakupkiUtils.ZakupkiFile(
                file.ParentDir,
                file.Name,
                file.Modified,
                file.Size,
                file.IsFile);
            await ZakupkiUtils.FtpZakupkiService.DownloadFile(f, targetLocalFile, progress, error);
        }

        public static async Task CopyStream(Stream from, Stream to, Action<long> progress)
        {
            await ZakupkiUtils.FtpZakupkiService.CopyStream(from, to, progress);
        }

    }
}
