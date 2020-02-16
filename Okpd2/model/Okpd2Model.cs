using Okpd2.infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ZakupkiUtils.infrastructure;

namespace Okpd2.model
{
    class Okpd2Model : INotifyPropertyChanged
    {
        public Okpd2Model(IZakupkiFactory factory)
        {
            _settings = factory.CreateSettings();
            _fileService = factory.CreteFileService();
            _localFileService = factory.CreateLocalFileService(_settings);
        }

        public void Close()
        {
            _isClose = true;
        }

        public bool HasOkpd2Changes
        {
            get => _hasOkpd2Changes;
            private set
            {
                _hasOkpd2Changes = value;
                OnPropertyChanged(nameof(HasOkpd2Changes));
            }
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
            bool result = false;
            using (var fw = new FalseWhile(SetIsAvailable))
            {
                HasOkpd2Changes = false;
                Progress = "Начата проверка ОКПД2";
                await Task.Run(() => CheckOkpd2Long(r => { result = r; }));
                Progress = "Проверка закончена";
                HasOkpd2Changes = result;
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

        internal void CheckOkpd2Long(Action<bool> result)
        {
            string localDir = _settings.GetLocalOkpd2Dir();
            IEnumerable<ZakupkiFile> localFiles = _localFileService.GetLocalFiles(localDir);
            IEnumerable<ZakupkiFile> files = _fileService.GetFiles(_settings.GetOkpd2Dir());
            bool isEquals = _localFileService.EqualsWithoutParent(localFiles, files);
            result(isEquals);
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
        private bool _hasOkpd2Changes = false;
        private string _progress = string.Empty;
        private bool _isClose = false;
        private IZakupkiFileService _fileService;
        private IZakupkiSettings _settings;
        private IZakupkiLocalFileService _localFileService;

    }
}
