using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TypeIT.FileTypes;
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
            navigationStore = new NavigationStore();

            navigationStore.CurrentViewModel = new DashboardViewModel(navigationStore);

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
