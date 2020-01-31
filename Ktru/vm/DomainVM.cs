using Ktru.infrastructure;
using System.ComponentModel;

namespace Ktru.vm
{
    class DomainVM : INotifyPropertyChanged
    {
        public DomainVM(IDomainModel m)
        {
            model = m;
            model.PropertyChanged += this.OnModelPropertyChanged;
        }

        public bool IsKtruChanged
        {
            get => model.IsKtruModified;
        }

        public string KtruText
        {
            get => ktruText;
            private set
            {
                if (value == ktruText)
                {
                    return;
                }
                ktruText = value;
                OnPropertyChanged("KtruText");
            }
        }

        public string KtruBackground
        {
            get => ktruBackground;
            private set
            {
                if (value == ktruBackground)
                {
                    return;
                }
                ktruBackground = value;
                OnPropertyChanged("KtruBackground");
            }
        }

        private string ktruText;
        private string ktruBackground = "White";

        private IDomainModel model;

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsKtruModified")
            {
                if (IsKtruChanged)
                {
                    KtruText = "ВНИМАНИЕ! Есть новые данные на сервере.";
                    KtruBackground = "LemonChiffon";
                }
                else
                {
                    KtruText = "Все локальные данные актуальны.";
                    KtruBackground = "White";
                }
                OnPropertyChanged("IsKtruChanged");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
