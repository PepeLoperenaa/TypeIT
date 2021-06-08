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
    class ChangeUserViewModelWithOneToFiveUsers : ChangeUserViewModel
    {
        public ICommand CreateUser { get; }


        public ChangeUserViewModelWithOneToFiveUsers(NavigationStore navigationStore) : base(navigationStore)
        {
            //Commands
            CreateUser = new DelegateCommand<string>(createUserFile);
        }

        private void createUserFile(string userName)
        {
            XmlHandler.newUser(userName);
            base.loadSelectedUser(userName);
        }
    }
}
