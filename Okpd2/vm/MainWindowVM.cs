using Okpd2.command;
using Okpd2.model;
using Okpd2.operation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using ZakupkiUtils.infrastructure;

namespace Okpd2.vm
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            model.PropertyChanged += OnModelPropertyChanged;
        }

        public DelegateCommand CheckOkpd2Command => new DelegateCommand(model.CheckOkpd2, CanExecuteCommands);
        public DelegateCommand LoadOkpd2Command => new DelegateCommand(model.LoadOkpd2, CanExecuteCommands);
        public DelegateCommand WindowClosing => new DelegateCommand(OnClose);

        public string CurrentInfo
        {
            get => _currentInfo;
            private set
            {
                _currentInfo = value;
                OnPropertyChanged(nameof(CurrentInfo));
            }
        }

        public string CurrentInfoBackground
        {
            get => _currentInfoBackground;
            private set
            {
                _currentInfoBackground = value;
                OnPropertyChanged(nameof(CurrentInfoBackground));
            }
        }

        private void OnClose(object o)
        {
            model.Close();
        }

        private static readonly IZakupkiFactory factory = new ZakupkiFactory();
        private readonly Okpd2Model model = new Okpd2Model(factory);

        private bool CanExecuteCommands(object o)
        {
            return model.IsAvailable;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(model.Progress))
            {
                Trace.Assert(sender == model);
                CurrentInfoBackground = "White";
                CurrentInfo = model.Progress;
            }
            else if (e.PropertyName == nameof(model.HasOkpd2Changes))
            {
                Trace.Assert(sender == model);
                if (model.HasOkpd2Changes)
                {
                    CurrentInfoBackground = "LemonChiffon";
                    CurrentInfo = "ВНИМАНИЕ! Есть новые данные на сервере.";
                }
                else
                {
                    CurrentInfoBackground = "White";
                    CurrentInfo = "Все локальные данные актуальны.";
                }
            }
        }

        private string _currentInfo = string.Empty;
        private string _currentInfoBackground = "White";

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
