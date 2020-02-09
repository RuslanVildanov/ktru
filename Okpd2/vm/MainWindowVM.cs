using Okpd2.command;
using Okpd2.infrastructure;
using Okpd2.model;
using Okpd2.operation;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Okpd2.vm
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            operationLayer = new Okpd2OperationLayer(model);
        }

        public DelegateCommand CheckOkpd2Command => new DelegateCommand(CheckOkpd2, CanExecuteCommands);
        public DelegateCommand WindowClosing => new DelegateCommand(OnClose);

        private async void CheckOkpd2(object o)
        {
            using (var fw = new FalseWhile(SetCanExecuteCommands))
            {
                await Task.Run(() => CheckOkpd2Long() );
            }
        }

        internal void CheckOkpd2Long()
        {
            for (var i = 0; i < 1000000; i++)
            {
                if (isClose)
                {
                    break;
                }
                Console.WriteLine(i);

                // the following line will block main thread unless
                //  ExecuteLongProcedure is called in an async method
                System.Threading.Thread.Sleep(1);
            }
        }

        private void OnClose(object o)
        {
            isClose = true;
        }

        private readonly Okpd2Model model = new Okpd2Model();
        private readonly Okpd2OperationLayer operationLayer;

        private bool CanExecuteCommands(object o)
        {
            return canExecuteCommands;
        }

        private bool canExecuteCommands = true;
        private void SetCanExecuteCommands(bool v)
        {
            canExecuteCommands = v;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isClose = false;
    }
}
