using Lab05_Tyshchenko.Models;
using Lab05_Tyshchenko.Navigation;
using Lab05_Tyshchenko.Tools;
using Lab05_Tyshchenko.Windows;
using System;
using System.Windows;

namespace Lab05_Tyshchenko
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ContentWindow contentWindow = new ContentWindow();

            NavigationModel navigationModel = new NavigationModel(contentWindow);
            NavigationManager.Instance.Initialize(navigationModel);
            contentWindow.Show();
            navigationModel.Navigate(ModesEnum.Main);

            Current.MainWindow.Closing += (s, a) =>
            {

                if (MessageBox.Show("Вийти?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    StationManager.InvokeStopThreads();
                    Environment.Exit(0);
                }
                else
                    a.Cancel = true;
            };

        }
    }
}
