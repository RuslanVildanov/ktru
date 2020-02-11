using Okpd2.command;
using Okpd2.model;
using Okpd2.operation;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Okpd2.vm
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            model.PropertyChanged += OnModelPropertyChanged;
            operationLayer = new Okpd2OperationLayer(model);
        }

        public DelegateCommand CheckOkpd2Command => new DelegateCommand(model.CheckOkpd2, CanExecuteCommands);
        public DelegateCommand LoadOkpd2Command => new DelegateCommand(model.LoadOkpd2, CanExecuteCommands);
        public DelegateCommand WindowClosing => new DelegateCommand(OnClose);

        private void OnClose(object o)
        {
            model.Close();
        }

        private readonly Okpd2Model model = new Okpd2Model();
        private readonly Okpd2OperationLayer operationLayer;

        private bool CanExecuteCommands(object o)
        {
            return model.IsAvailable;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(model.Progress))
            {
                Trace.Assert(sender == model);
                Console.WriteLine(model.Progress);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
