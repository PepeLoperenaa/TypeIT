using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    public abstract class ChangeUserViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand SelectUser { get; }
        public UserStore CurrentUser { get; set; }
        public List<UserModel> Users { get; set; }
        public ICommand ExitCommand { get; set; }

        public ChangeUserViewModel(NavigationStore navigationStore)
        {
            //Creating a new user model
            CurrentUser = new UserStore();

            //Navigate home
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, CurrentUser));

            //Commands
            SelectUser = new DelegateCommand<string>(LoadSelectedUser);
            ExitCommand = new DelegateCommand(ClickedExit);

            //Users list
            Users = new List<UserModel>();
            LoadUsers();
        }

        /// <summary>
        /// Loads the selected user in the user view
        /// </summary>
        /// <param name="userName">The user's name to be loaded</param>
        protected void LoadSelectedUser(string userName)
        {
            //Create a new UserModel
            UserModel user = new(userName, true);

            //Setting the current user to the selected one
            CurrentUser.CurrentUser = user;

            //Setting the default theme from the user's .TpyeIT
            Application.Current.Resources.MergedDictionaries.Clear();

            //If the theme is unset then set it to light
            //Theme should NOT be unset
            string theme = (user.Theme == "") ? "Light" : user.Theme;

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/" + theme + "Theme.xaml", UriKind.Relative) });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/StyleResource.xaml", UriKind.Relative) });
                        
            //Navigate to dashboard
            NavigateHomeCommand.Execute(null);
        }


        /// <summary>
        /// Loads all of the users in the LogIn page
        /// </summary>
        protected void LoadUsers()
        {
            string[] files = Directory.GetFiles("../../../FileTypes/Users");
            foreach (string file in files)
            {
                string userName = Path.GetFileName(file);
                int pos = userName.LastIndexOf(".");
                userName = userName.Remove(pos);
                UserModel user = new UserModel(userName, false);
                Users.Add(user);
            }
        }

        /// <summary>
        /// Opens a dialog box
        /// If yes is clicked the application quits
        /// </summary>
        protected static void ClickedExit()
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
