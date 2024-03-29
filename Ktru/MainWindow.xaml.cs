﻿using Ktru.infrastructure;
using Ktru.model;
using Ktru.operation;
using Ktru.vm;
using Ktru.xlsx;
using System;
using System.Diagnostics;
using System.Windows;
using ZakupkiUtils.infrastructure;

namespace Ktru
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IZakupkiFactory factory = new ZakupkiFactory();
        private static IZakupkiSettings settings = factory.CreateSettings();
        private static IDomainModel domainModel = new DomainModel(factory.CreateLocalFileService(settings));
//        private static IXlsxOperation xlsx = new ExcelLibraryOperation();
        private static IXlsxOperation xlsx = new EPPlusOperation();
        private OperationLayer operationLayer = new OperationLayer(
            factory.CreteFileService(),
            settings,
            xlsx,
            domainModel);

        public MainWindow()
        {
            DataContext = new DomainVM(domainModel);
            InitializeComponent();
            operationLayer.CheckKtruRelevance();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BuildAllKtru_Click(object sender, RoutedEventArgs e)
        {
            bool onlyActual = false;
            string error = string.Empty;
            var resultFile = settings.GetResultXlsx(onlyActual);

            checkKtruMainMenu.IsEnabled = false;
            updateKtruMainMenu.IsEnabled = false;
            buildAllKtruMainMenu.IsEnabled = false;
            buildActualKtruMainMenu.IsEnabled = false;

            await operationLayer.BuildKtruFile(resultFile, f =>
            {
                progressText.Text = f.TextInfo + ": " + f.ZF.Name + ". Обработано " + f.Progress + " / " + f.ZF.Size;
            },
            r => { error = r; },
            onlyActual,
            false);

            checkKtruMainMenu.IsEnabled = true;
            updateKtruMainMenu.IsEnabled = true;
            buildAllKtruMainMenu.IsEnabled = true;
            buildActualKtruMainMenu.IsEnabled = true;

            progressText.Text = "";
            if (error != string.Empty)
            {
                MessageBox.Show(
                    "Произошла ошибка при построении КТРУ-файла.\n" + error,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                var r = MessageBox.Show(
                    resultFile + "\n Открыть?",
                    "Успешно создан файл",
                    MessageBoxButton.YesNo);
                if (r == MessageBoxResult.Yes)
                {
                    try
                    {
                        Process.Start(resultFile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        MessageBox.Show(
                            "Произошла ошибка при открытии файла.\n" + ex.Message,
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void UpdateKtru_Click(object sender, RoutedEventArgs e)
        {
            string error = string.Empty;

            checkKtruMainMenu.IsEnabled = false;
            updateKtruMainMenu.IsEnabled = false;
            buildAllKtruMainMenu.IsEnabled = false;
            buildActualKtruMainMenu.IsEnabled = false;

            await operationLayer.UpdateKtru(f =>
            {
                progressText.Text = f.TextInfo + ": " + f.ZF.Name + ". Загружено " + f.Progress + " / " + f.ZF.Size;
            },
            r => { error = r; });

            checkKtruMainMenu.IsEnabled = true;
            updateKtruMainMenu.IsEnabled = true;
            buildAllKtruMainMenu.IsEnabled = true;
            buildActualKtruMainMenu.IsEnabled = true;

            progressText.Text = "";
            operationLayer.CheckKtruRelevance();
            if (error != string.Empty)
            {
                MessageBox.Show(
                    "Произошла ошибка при попытке загрузить данные с сервера.\n" + error,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void CheckKtru_Click(object sender, RoutedEventArgs e)
        {
            operationLayer.CheckKtruRelevance();
        }

        private async void BuildActualKtru_Click(object sender, RoutedEventArgs e)
        {
            bool onlyActual = true;
            string error = string.Empty;
            var resultFile = settings.GetResultXlsx(onlyActual);

            checkKtruMainMenu.IsEnabled = false;
            updateKtruMainMenu.IsEnabled = false;
            buildAllKtruMainMenu.IsEnabled = false;
            buildActualKtruMainMenu.IsEnabled = false;

            await operationLayer.BuildKtruFile(resultFile, f =>
            {
                progressText.Text = f.TextInfo + ": " + f.ZF.Name + ". Обработано " + f.Progress + " / " + f.ZF.Size;
            },
            r => { error = r; },
            onlyActual,
            false);

            checkKtruMainMenu.IsEnabled = true;
            updateKtruMainMenu.IsEnabled = true;
            buildAllKtruMainMenu.IsEnabled = true;
            buildActualKtruMainMenu.IsEnabled = true;

            progressText.Text = "";
            if (error != string.Empty)
            {
                MessageBox.Show(
                    "Произошла ошибка при построении КТРУ-файла.\n" + error,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                var r = MessageBox.Show(
                    resultFile + "\n Открыть?",
                    "Успешно создан файл",
                    MessageBoxButton.YesNo);
                if (r == MessageBoxResult.Yes)
                {
                    try
                    {
                        Process.Start(resultFile);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        MessageBox.Show(
                            "Произошла ошибка при открытии файла.\n" + ex.Message,
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
