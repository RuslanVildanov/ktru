using Okpd2.infrastructure;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Okpd2.model
{
    class Okpd2Model : INotifyPropertyChanged
    {
        public void Close()
        {
            _isClose = true;
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            private set
            {
                _isAvailable = value;
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        public string Progress
        {
            get => _progress;
            private set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public async void CheckOkpd2(object o)
        {
            using (var fw = new FalseWhile(SetIsAvailable))
            {
                Progress = "Начало работы";
                await Task.Run(() => CheckOkpd2Long());
                Progress = "Работа закончена";
            }
        }

        public async void LoadOkpd2(object o)
        {
            using (var fw = new FalseWhile(SetIsAvailable))
            {
                Progress = "Начало работы";
                await Task.Run(() => LoadOkpd2Long());
                Progress = "Работа закончена";
            }
        }

        internal void CheckOkpd2Long()
        {
            for (var i = 0; i < 10000; i++)
            {
                if (_isClose)
                {
                    break;
                }
                Console.WriteLine(i);

                if (i == 5000)
                {
                    Progress = "середина работы";
                }

                // the following line will block main thread unless
                //  ExecuteLongProcedure is called in an async method
                System.Threading.Thread.Sleep(1);
            }
        }

        internal void LoadOkpd2Long()
        {
            Progress = "середина работы";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetIsAvailable(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }
        private bool _isAvailable = true;
        private string _progress = string.Empty;
        private bool _isClose = false;

    }
}
