using Prism.Commands;
using System;
using System.Collections.Generic;
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
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
            NavigateChangeUserCommand = new NavigateCommand<ChangeUserViewModel>(navigationStore, () => new ChangeUserViewModel(navigationStore));
            NavigateChangeGameModeCommand = new NavigateCommand<ChangeGameModeViewModel>(navigationStore, () => new ChangeGameModeViewModel(navigationStore));
            //ResetStatisticsCommand
            ChangeThemeCommand = new DelegateCommand(ClickedChangeTheme);
            //DeleteAccountCommand
            ExitCommand = new DelegateCommand(ClickedExit);
            currentTheme = "LightTheme.xaml";
        }

        private void ClickedExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ClickedChangeTheme()
        {
            currentTheme = "LightTheme.xaml";
            System.Uri uri  = Application.Current.Resources.MergedDictionaries[0].Source;



            string gg = uri.ToString();

            System.Uri uriNew = new Uri("Resources/LightTheme.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries[0].Source = uriNew;


        }

    }
}
