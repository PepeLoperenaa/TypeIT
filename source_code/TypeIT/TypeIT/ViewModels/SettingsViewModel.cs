using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand ChangeUserCommand { get; set; }
        public ICommand NavigateChangeGameModeCommand { get; set; }
        public ICommand ResetStatisticsCommand { get; set; }
        public ICommand ChangeThemeCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public SettingsViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
            //ChangeUserCommand
            NavigateChangeGameModeCommand = new NavigateCommand<ChangeGameModeViewModel>(navigationStore, () => new ChangeGameModeViewModel(navigationStore));
            //ResetStatisticsCommand
            ChangeThemeCommand = new DelegateCommand(ClickedChangeTheme);
            //DeleteAccountCommand
            ExitCommand = new DelegateCommand(ClickedExit);
        }

        private void ClickedExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ClickedChangeTheme()
        {
            
        }
    }
}
