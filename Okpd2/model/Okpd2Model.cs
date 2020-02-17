using Okpd2.infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
            using (var fw = new FalseWhile(SetIsAvailable))
            {
                await Task.Run(() => CheckOkpd2Long());
            }
        }

        public async void LoadOkpd2(object o)
        {
            using (var fw = new FalseWhile(SetIsAvailable))
            {
                Progress = "Начата загрузка файлов ОКПД2";
                bool hasError = false;
                await LoadOkpd2Long(err => { hasError = err; });
                if (!hasError)
                {
                    Progress = "Загрузка файлов ОКПД2 закончена";
                }
            }
        }

        internal void CheckOkpd2Long()
        {
            HasOkpd2Changes = false;
            Progress = "Начата проверка ОКПД2";
            System.Threading.Thread.Sleep(500); //это для того, чтобы на экране отобразился прогресс

            string localDir = _settings.GetLocalOkpd2Dir();
            IEnumerable<ZakupkiFile> localFiles = _localFileService.GetLocalFiles(localDir);
            IEnumerable<ZakupkiFile> files = _fileService.GetFiles(_settings.GetOkpd2Dir());
            bool isEquals = _localFileService.EqualsWithoutParent(localFiles, files);
            Progress = "Проверка закончена";
            System.Threading.Thread.Sleep(1000);

            HasOkpd2Changes = !isEquals;
        }

        internal async Task LoadOkpd2Long(Action<bool> hasErrorAction)
        {
            string localOkpd2Dir = _settings.CreateLocalOkpd2DirIfNeed(out string error);
            if (error != string.Empty)
            {
                Progress =  error;
                return;
            }
            IEnumerable<ZakupkiFile> zakupkiFiles = _fileService.GetFiles(_settings.GetOkpd2Dir());
            bool hasError = false;
            foreach (var zakupkiFile in zakupkiFiles)
            {
                await DownloadFile(localOkpd2Dir, zakupkiFile, err => {
                    if(err != string.Empty)
                    {
                        Progress = err;
                        hasError = true;
                    }
                });
                if (hasError || _isClose)
                {
                    break;
                }
            }
            if (!hasError && !_isClose)
            {
                _localFileService.RemoveNotFoundLocalFiles(localOkpd2Dir, zakupkiFiles, out error);
                if (error != string.Empty)
                {
                    Progress = error;
                }
                await ExtractLocalOkpd2Files(localOkpd2Dir, err => { hasError = err; });
            }
            hasErrorAction(hasError);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task DownloadFile(
            string targetDir,
            ZakupkiFile file,
            Action<string> error)
        {
            string localFile = targetDir + '\\' + file.Name;
            var f = _localFileService.GetLocalFile(localFile, out bool ok);
            if (ok && file.EqualsWithoutParent(f))
            {
                return;
            }
            await _fileService.DownloadFile(file, localFile,
                progress => {
                    Progress = "Удалённая загрузка файла " + file.Name + " загружено " + progress + "/" + file.Size;
                }, error);
        }

        private async Task ExtractLocalOkpd2Files(string localOkpd2Dir, Action<bool> hasErrorAction)
        {
            string archiveDir = _settings.PrepareLocalOkpd2ArchiveDir(out string error);
            if (error != string.Empty)
            {
                Progress = error;
                hasErrorAction(true);
                return;
            }
            await _localFileService.ExtractLocalZipFiles(_settings.GetLocalOkpd2Dir(), archiveDir,
                progress =>
                {
                    Progress = "Распаковка файла " + progress;
                },
                err =>
                {
                    Progress = err;
                    hasErrorAction(true);
                });
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
