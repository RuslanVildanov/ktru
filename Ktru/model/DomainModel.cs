using Ktru.infrastructure;
using System.Collections.Generic;
using System.ComponentModel;
using ZakupkiUtils.infrastructure;

namespace Ktru.model
{
    class DomainModel : IDomainModel
    {
        public DomainModel(IZakupkiLocalFileService localFileService)
        {
            _localFileService = localFileService;
        }

        public bool IsKtruModified
        {
            get => isKtruModified;
            set
            {
                isKtruModified = value;
                OnPropertyChanged("IsKtruModified");
            }
        }

        public IEnumerable<ZakupkiFile> GetLocalFiles(string localDir)
        {
            return _localFileService.GetLocalFiles(localDir);
        }

        public ZakupkiFile GetLocalFile(string localFile, out bool ok)
        {
            return _localFileService.GetLocalFile(localFile, out ok);
        }

        public void RemoveNotFoundLocalFiles(string localDir, IEnumerable<ZakupkiFile> notFoundIn, out string error)
        {
            _localFileService.RemoveNotFoundLocalFiles(localDir, notFoundIn, out error);
        }

        public bool EqualsWithoutParent(IEnumerable<ZakupkiFile> f1, IEnumerable<ZakupkiFile> f2)
        {
            return _localFileService.EqualsWithoutParent(f1, f2);
        }

        private bool isKtruModified = false;

        private readonly IZakupkiLocalFileService _localFileService;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
