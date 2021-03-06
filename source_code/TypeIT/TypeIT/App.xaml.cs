using System;
using System.IO;
using System.Windows;
using TypeIT.Stores;
using TypeIT.ViewModels;

namespace TypeIT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NavigationStore navigationStore { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            string[] files = Directory.GetFiles("../../../FileTypes/Users");

            navigationStore = new NavigationStore();

            if (files.Length < 6)
            {
                navigationStore.CurrentViewModel = new ChangeUserViewModelWithOneToFiveUsers(navigationStore);
            }
            else
            {
                navigationStore.CurrentViewModel = new ChangeUserViewModelWithSixUsers(navigationStore);
            }


            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            //Setting the default theme
            System.Uri uriNew = new Uri("Resources/LightTheme.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries[0].Source = uriNew;

            MainWindow.Show();
            base.OnStartup(e);
        }


    }
}
