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

        public UserStore currentUser { get; set; }

        public DashboardViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateAboutCommand = new NavigateCommand<AboutViewModel>(navigationStore, () => new AboutViewModel(navigationStore, userStore));
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationStore, () => new SettingsViewModel(navigationStore, userStore));
            NavigateDocumentsCommand = new NavigateCommand<DocumentsViewModel>(navigationStore, () => new DocumentsViewModel(navigationStore, userStore));
            NavigateStatisticsCommand = new NavigateCommand<StatisticsViewModel>(navigationStore, () => new StatisticsViewModel(navigationStore, userStore));
            NavigateTypingCommand = new NavigateCommand<TypingViewModel>(navigationStore, () => new TypingViewModel(navigationStore, userStore));
            NavigateAchievementsCommand = new NavigateCommand<AchievementsViewModel>(navigationStore, () => new AchievementsViewModel(navigationStore, userStore));
            ExitCommand = new DelegateCommand(ClickedExit);

            currentUser = userStore;
        }


        /// <summary>
        /// Quitting the application 
        /// </summary>
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