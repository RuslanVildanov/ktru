using System.Collections.Generic;
using ZakupkiUtils.infrastructure;

namespace Ktru.infrastructure
{
    interface IDomainRepository
    {
        IEnumerable<ZakupkiFile> GetLocalFiles(string localDir);
        ZakupkiFile GetLocalFile(string localFile, out bool ok);
    }
}
