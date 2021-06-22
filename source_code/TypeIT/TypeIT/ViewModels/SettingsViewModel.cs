using Prism.Commands;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    internal class SettingsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateSettingsCommand { get; }
        public ICommand NavigateChangeUserCommandForFiveUsers { get; set; }
        public ICommand NavigateChangeUserCommandForSixUsers { get; set; }
        public ICommand NavigateChangeGameModeCommand { get; set; }
        public ICommand ResetStatisticsCommand { get; set; }
        public ICommand ChangeThemeCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand ChangeUserCommand { get; set; }
        public string CurrentTheme { get; set; }
        public UserStore CurrentUser { get; set; }

        public SettingsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            //Navigation comands
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            NavigateChangeUserCommandForFiveUsers = new NavigateCommand<ChangeUserViewModelWithOneToFiveUsers>(navigationStore, () => new ChangeUserViewModelWithOneToFiveUsers(navigationStore));
            NavigateChangeUserCommandForSixUsers = new NavigateCommand<ChangeUserViewModelWithSixUsers>(navigationStore, () => new ChangeUserViewModelWithSixUsers(navigationStore));
            NavigateChangeGameModeCommand = new NavigateCommand<ChangeGameModeViewModel>(navigationStore, () => new ChangeGameModeViewModel(navigationStore, userStore));
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationStore, () => new SettingsViewModel(navigationStore, userStore));

            //Delete User Command
            DeleteAccountCommand = new DelegateCommand(ClickedDeleteAccount);
            //Change User Command
            ChangeUserCommand = new DelegateCommand(ClickedChangeUser);
            //Current user
            CurrentUser = userStore;
            //Change Theme Command
            ChangeThemeCommand = new DelegateCommand(ClickedChangeTheme);
            //Reset Statistics Command
            ResetStatisticsCommand = new DelegateCommand(ClickedResetStatistics);
            //Exit Command
            ExitCommand = new DelegateCommand(ClickedExit);
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

        /// <summary>
        /// Changing the theme of the application
        /// Switches between Light and Dark theme
        /// </summary>
        private void ClickedChangeTheme()
        {
            string currentTheme = Application.Current.Resources.MergedDictionaries[0].Source.ToString();

            Application.Current.Resources.MergedDictionaries.Clear();

            if (currentTheme == "Resources/LightTheme.xaml")
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/DarkTheme.xaml", UriKind.Relative) });
                XmlHandler.UpdateSettings(CurrentUser.CurrentUser.Name, "Theme", "Dark");
            }
            else if (currentTheme == "Resources/DarkTheme.xaml")
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/LightTheme.xaml", UriKind.Relative) });
                XmlHandler.UpdateSettings(CurrentUser.CurrentUser.Name, "Theme", "Light");
            }

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/StyleResource.xaml", UriKind.Relative) });

            NavigateSettingsCommand.Execute(null);
        }

        /// <summary>
        /// Deleting the current user
        /// Shows an alert asking if the user wants to delete their account as it is irreversible
        /// </summary>
        private void ClickedDeleteAccount()
        {
            string usersFolder = "../../../FileTypes/Users/";
            string userToDelete = CurrentUser.CurrentUser.Name + ".TypeIT";

            var res = Xceed.Wpf.Toolkit.MessageBox.Show(
                "Are you sure you want to delete your account? This is irreversible!",
                "Delete Account",
                MessageBoxButton.YesNo
            );

            if ("Yes" == res.ToString())
            {
                if (File.Exists(Path.Combine(usersFolder, userToDelete)))
                {
                    File.Delete(Path.Combine(usersFolder, userToDelete));
                    NavigateSettingsCommand.Execute(null);
                    ChangeUserCommand.Execute(null);
                }
            }
        }

        /// <summary>
        /// Resetting the current users statistics
        /// Shows an alert asking if the usre wants reset their statistics as it is irreversible
        /// </summary>
        private void ClickedResetStatistics()
        {
            var res = Xceed.Wpf.Toolkit.MessageBox.Show(
                "Are you sure you want to reset your statistics? This is irreversible!",
                "Reset Statistics",
                MessageBoxButton.YesNo
            );

            if ("Yes" == res.ToString())
            {
                XmlHandler.ClearUserStatistics(CurrentUser.CurrentUser.Name);
            }

        }

        /// <summary>
        /// Changing the current user
        /// Directs you to separate pages based upon the number of current users
        /// </summary>
        private void ClickedChangeUser()
        {
            string[] files = Directory.GetFiles("../../../FileTypes/Users");

            if (files.Length < 6)
            {
                NavigateChangeUserCommandForFiveUsers.Execute(null);
            }
            else
            {
                NavigateChangeUserCommandForSixUsers.Execute(null);
            }
        }
    }
}
