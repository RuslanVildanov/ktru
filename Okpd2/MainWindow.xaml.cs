using System.Windows;

namespace Okpd2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти из приложения?", "",
            //                              MessageBoxButton.OKCancel);
            //if (result == MessageBoxResult.Cancel)
            //{
            //    e.Cancel = true;
            //}
            base.OnClosing(e);
        }

    }
}
