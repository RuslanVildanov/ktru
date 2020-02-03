using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Okpd2.infrastructure
{
    interface IZakupkiFileService
    {
        IEnumerable<ZakupkiFile> GetFiles(string zakupki_dir);
        Task DownloadFile(
            ZakupkiFile file,
            string targetLocalFile,
            Action<long> progress,
            Action<string> error);
    }
}
