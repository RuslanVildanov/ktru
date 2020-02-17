using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZakupkiUtils.infrastructure
{
    public interface IZakupkiLocalFileService
    {
        IEnumerable<ZakupkiFile> GetLocalFiles(string localDir);
        ZakupkiFile GetLocalFile(string localFile, out bool ok);
        void RemoveNotFoundLocalFiles(string localDir, IEnumerable<ZakupkiFile> notFoundIn, out string error);
        bool EqualsWithoutParent(IEnumerable<ZakupkiFile> f1, IEnumerable<ZakupkiFile> f2);
        Task ExtractLocalZipFiles(
            string zipFilesDir,
            string targetDir,
            Action<string> progress,
            Action<string> error);

    }
}
