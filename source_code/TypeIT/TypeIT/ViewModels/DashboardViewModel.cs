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
    class DashboardViewModel : ViewModelBase
    {
        public ICommand NavigateAboutCommand { get; }
        public ICommand ExitCommand { get; set; }
        public ICommand NavigateSettingsCommand { get; set; }
        public ICommand NavigateDocumentsCommand { get; set; }
        public ICommand NavigateStatisticsCommand { get; set; }
        public ICommand NavigateTypingCommand { get; set; }
        public ICommand NavigateAchievementsCommand { get; set; }

        public DashboardViewModel(NavigationStore navigationStore)
        {
            NavigateAboutCommand = new NavigateCommand<AboutViewModel>(navigationStore, () => new AboutViewModel(navigationStore));
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationStore, () => new SettingsViewModel(navigationStore));
            NavigateDocumentsCommand = new NavigateCommand<DocumentsViewModel>(navigationStore, () => new DocumentsViewModel(navigationStore));
            NavigateStatisticsCommand = new NavigateCommand<StatisticsViewModel>(navigationStore, () => new StatisticsViewModel(navigationStore));
            NavigateTypingCommand = new NavigateCommand<TypingViewModel>(navigationStore, () => new TypingViewModel(navigationStore));
            NavigateAchievementsCommand = new NavigateCommand<AchievementsViewModel>(navigationStore, () => new AchievementsViewModel(navigationStore));
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
    }
}