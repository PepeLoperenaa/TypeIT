using Prism.Commands;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;
using TypeIT.Utilities;

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
        public ICommand StartTypingCommand { get; set; }
        public UserStore CurrentUser { get; set; }
        public DocumentModel CurrentDocument { get; set; }

        /// <summary>
        /// Contains the navigation ICommands to different views
        /// </summary>
        /// <param name="navigationStore">Stores the current view</param>
        /// <param name="userStore">Stores the current view</param>
        public DashboardViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            //Navigating to About view
            NavigateAboutCommand = new NavigateCommand<AboutViewModel>(navigationStore, () => new AboutViewModel(navigationStore, userStore));

            //Navigating to Settings view
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationStore, () => new SettingsViewModel(navigationStore, userStore));

            //Navigate to Documents view
            NavigateDocumentsCommand = new NavigateCommand<DocumentsViewModel>(navigationStore, () => new DocumentsViewModel(navigationStore, userStore));

            //Naviagte to Statistics view
            NavigateStatisticsCommand = new NavigateCommand<StatisticsViewModel>(navigationStore, () => new StatisticsViewModel(navigationStore, userStore));

            //Navigate to Typing view
            NavigateTypingCommand = new NavigateCommand<TypingViewModel>(navigationStore, () => new TypingViewModel(navigationStore, userStore, CurrentDocument));

            //Navigate to Achievements view
            NavigateAchievementsCommand = new NavigateCommand<AchievementsViewModel>(navigationStore, () => new AchievementsViewModel(navigationStore, userStore));

            //Command when exit button is clicked
            ExitCommand = new DelegateCommand(ClickedExit);

            StartTypingCommand = new DelegateCommand(ClickedStartTyping);

            //Setting the current user to the parameter
            CurrentUser = userStore;
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

        private void ClickedStartTyping()
        {
            CurrentDocument = XmlHandler.GetRandomUserDocument(CurrentUser.CurrentUser.Name);
            if (CurrentDocument != null)
            {
                NavigateTypingCommand.Execute(null);
            }
        }
    }
}