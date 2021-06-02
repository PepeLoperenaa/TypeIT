using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateChangeUserCommand { get; set; }
        public ICommand NavigateChangeGameModeCommand { get; set; }
        public ICommand ResetStatisticsCommand { get; set; }
        public ICommand ChangeThemeCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        
        public string currentTheme { get; set; }

        public SettingsViewModel(NavigationStore navigationStore)
        {
            //Navigation comands
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
            NavigateChangeUserCommand = new NavigateCommand<ChangeUserViewModel>(navigationStore, () => new ChangeUserViewModel(navigationStore));
            NavigateChangeGameModeCommand = new NavigateCommand<ChangeGameModeViewModel>(navigationStore, () => new ChangeGameModeViewModel(navigationStore));

            //ResetStatisticsCommand
            ChangeThemeCommand = new DelegateCommand(ClickedChangeTheme);

            //DeleteAccountCommand
            ExitCommand = new DelegateCommand(ClickedExit);
        }

        private void ClickedExit()
        {
            //Custom messagebox
            var res = Xceed.Wpf.Toolkit.MessageBox.Show(
                           "Are you sure you want to quit?",
                           "Quit",
                           MessageBoxButton.YesNo
                       );

            if ("Yes" == res.ToString())
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void ClickedChangeTheme()
        {

            string currentTheme = Application.Current.Resources.MergedDictionaries[0].Source.ToString();

            Application.Current.Resources.MergedDictionaries.Clear();

            if (currentTheme == "Resources/LightTheme.xaml")
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/DarkTheme.xaml", UriKind.Relative) });
            } else if (currentTheme == "Resources/DarkTheme.xaml")
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/LightTheme.xaml", UriKind.Relative) });
            }

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/StyleResource.xaml", UriKind.Relative) });

            NavigateHomeCommand.Execute(null);
        }

    }
}
