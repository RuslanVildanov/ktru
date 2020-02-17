using System.Collections.Generic;
using System.ComponentModel;
using ZakupkiUtils.infrastructure;

namespace Ktru.infrastructure
{
    interface IDomainModel : INotifyPropertyChanged
    {
        bool IsKtruModified { get; set; }
        IEnumerable<ZakupkiFile> GetLocalFiles(string localDir);
        ZakupkiFile GetLocalFile(string localFile, out bool ok);
        void RemoveNotFoundLocalFiles(string localDir, IEnumerable<ZakupkiFile> notFoundIn, out string error);
        bool EqualsWithoutParent(IEnumerable<ZakupkiFile> f1, IEnumerable<ZakupkiFile> f2);
    }
}
