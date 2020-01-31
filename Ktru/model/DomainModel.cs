using Ktru.infrastructure;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ktru.model
{
    class DomainModel : IDomainModel
    {
        public DomainModel(IDomainRepository domainRepository)
        {
            repository = domainRepository;
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
            return repository.GetLocalFiles(localDir);
        }

        public ZakupkiFile GetLocalFile(string localFile, out bool ok)
        {
            return repository.GetLocalFile(localFile, out ok);
        }

        private bool isKtruModified = false;

        private readonly IDomainRepository repository;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
