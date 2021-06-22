using Prism.Commands;
using System.Windows.Input;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    class ChangeUserViewModelWithOneToFiveUsers : ChangeUserViewModel
    {
        public ICommand CreateUser { get; }

        /// <summary>
        /// Changing from user to another without exiting the application
        /// </summary>
        /// <param name="navigationStore"></param>
        public ChangeUserViewModelWithOneToFiveUsers(NavigationStore navigationStore) : base(navigationStore)
        {
            //Commands
            CreateUser = new DelegateCommand<string>(createUserFile);
        }

        private void createUserFile(string userName)
        {
            XmlHandler.NewUser(userName);
            base.loadSelectedUser(userName);
        }
    }
}
