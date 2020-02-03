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
    }
}
