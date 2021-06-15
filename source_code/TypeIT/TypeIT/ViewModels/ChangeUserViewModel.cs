using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.FileTypes;
using TypeIT.Models;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    public abstract class ChangeUserViewModel: ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand SelectUser { get; }
        public UserStore currentUser { get; set; }
        public List<UserModel> Users { get; set; }
        public ICommand ExitCommand { get; set; }


        public ChangeUserViewModel(NavigationStore navigationStore)
        {
            //Creating a new user model
            currentUser = new UserStore();

            //Navigate home
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, currentUser));

            //Commands
            SelectUser = new DelegateCommand<string>(loadSelectedUser);
            ExitCommand = new DelegateCommand(ClickedExit);

            //Users list
            Users = new List<UserModel>();
            loadUsers();
        }

        protected void loadSelectedUser(string userName)
        {
            UserModel user = new UserModel(userName, true);

            //Setting the current user to the selected one
            currentUser.CurrentUser = user;

            //Setting the default theme
            string currentTheme = Application.Current.Resources.MergedDictionaries[0].Source.ToString();

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/" + user.Theme + "Theme.xaml", UriKind.Relative) });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Resources/StyleResource.xaml", UriKind.Relative) });

            //Navigate to dashboard
            NavigateHomeCommand.Execute(null);
        }

        protected void loadUsers()
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

        protected void ClickedExit()
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
