using Ktru.model;
using System.Collections.Generic;

namespace Ktru.infrastructure
{
    interface IDomainRepository
    {
        IEnumerable<ZakupkiFile> GetLocalFiles(string localDir);
        ZakupkiFile GetLocalFile(string localFile, out bool ok);
    }
}
