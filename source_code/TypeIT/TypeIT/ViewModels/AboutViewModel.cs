using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class AboutViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public UserStore CurrentUser { get; set; }

        public AboutViewModel(NavigationStore NavigationStore, UserStore UserStore)
        {
           NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(NavigationStore, () => new DashboardViewModel(NavigationStore, UserStore));
           CurrentUser = UserStore;
        }
    }
}
