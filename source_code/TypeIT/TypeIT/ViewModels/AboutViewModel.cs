using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    internal class AboutViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public UserStore CurrentUser { get; set; }

        public AboutViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            CurrentUser = userStore;
        }
    }
}
