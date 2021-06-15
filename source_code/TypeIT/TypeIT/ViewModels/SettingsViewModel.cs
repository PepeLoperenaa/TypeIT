using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        public UserStore currentUser { get; set; }

        public SettingsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            string[] files = Directory.GetFiles("../../../FileTypes/Users");

            if (files.Length < 6)
            {
                NavigateChangeUserCommand = new NavigateCommand<ChangeUserViewModelWithOneToFiveUsers>(navigationStore, () => new ChangeUserViewModelWithOneToFiveUsers(navigationStore));
            }
            else
            {
                NavigateChangeUserCommand = new NavigateCommand<ChangeUserViewModelWithSixUsers>(navigationStore, () => new ChangeUserViewModelWithSixUsers(navigationStore));
            }
            //Navigation comands
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
           
            NavigateChangeGameModeCommand = new NavigateCommand<ChangeGameModeViewModel>(navigationStore, () => new ChangeGameModeViewModel(navigationStore, userStore));

            //Delete User Command
            DeleteAccountCommand = new DelegateCommand(ClickedDeleteAccount);

            //Current user
            currentUser = userStore;

            //ResetStatisticsCommand
            ChangeThemeCommand = new DelegateCommand(ClickedChangeTheme);

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
        /// Changing the theme of the application. Can choose between Dark mode and Light mode. 
        /// </summary>
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

        private void ClickedDeleteAccount()
        {
            string usersFolder = "../../../FileTypes/Users/";
            string userToDelete = currentUser.CurrentUser.Name + ".TypeIT";

                        var res = Xceed.Wpf.Toolkit.MessageBox.Show(
                           "Are you sure you want to delete your account this is irreversible",
                           "Delete Account",
                           MessageBoxButton.YesNo
                       );

            if ("Yes" == res.ToString())
            {
                if (File.Exists(Path.Combine(usersFolder, userToDelete)))
                {
                    File.Delete(Path.Combine(usersFolder, userToDelete));
                    NavigateChangeUserCommand.Execute(null);
                }
            }

            

        }

    }
}
