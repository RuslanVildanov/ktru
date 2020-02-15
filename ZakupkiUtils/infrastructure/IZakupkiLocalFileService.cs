using System.Collections.Generic;

namespace ZakupkiUtils.infrastructure
{
    public interface IZakupkiLocalFileService
    {
        IEnumerable<ZakupkiFile> GetLocalFiles(string localDir);
        ZakupkiFile GetLocalFile(string localFile, out bool ok);
        bool EqualsWithoutParent(IEnumerable<ZakupkiFile> f1, IEnumerable<ZakupkiFile> f2);
    }
}
