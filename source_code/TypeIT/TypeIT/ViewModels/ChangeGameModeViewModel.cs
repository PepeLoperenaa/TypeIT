using Prism.Commands;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    class ChangeGameModeViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateSettingsCommand { get; }
        public ICommand ChangeGameModeToCasual { get; set; }
        public ICommand ChangeGameModeToNormal { get; set; }
        public ICommand ChangeGameModeToHard { get; set; }
        public ICommand ChangeGameModeToExtreme { get; set; }
        public UserStore CurrentUser { get; set; }

        public ChangeGameModeViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationStore, () => new SettingsViewModel(navigationStore, userStore));
            CurrentUser = userStore;

            ChangeGameModeToCasual = new DelegateCommand(setToCasual);
            ChangeGameModeToNormal = new DelegateCommand(setToNormal);
            ChangeGameModeToHard = new DelegateCommand(setToHard);
            ChangeGameModeToExtreme = new DelegateCommand(setToExtreme);
        }

        private void setToCasual()
        {
            XmlHandler.UpdateSettings(CurrentUser.CurrentUser.Name, "GameMode", "Casual");
            CurrentUser.CurrentUser.GameMode = Difficulty.Easy;
            NavigateSettingsCommand.Execute(null);
        }

        private void setToNormal()
        {
            XmlHandler.UpdateSettings(CurrentUser.CurrentUser.Name, "GameMode", "Normal");
            CurrentUser.CurrentUser.GameMode = Difficulty.Medium;
            NavigateSettingsCommand.Execute(null);
        }

        private void setToHard()
        {
            XmlHandler.UpdateSettings(CurrentUser.CurrentUser.Name, "GameMode", "Hard");
            CurrentUser.CurrentUser.GameMode = Difficulty.Hard;
            NavigateSettingsCommand.Execute(null);
        }

        private void setToExtreme()
        {
            XmlHandler.UpdateSettings(CurrentUser.CurrentUser.Name, "GameMode", "Extreme");
            CurrentUser.CurrentUser.GameMode = Difficulty.Extreme;
            NavigateSettingsCommand.Execute(null);
        }
    }
}
