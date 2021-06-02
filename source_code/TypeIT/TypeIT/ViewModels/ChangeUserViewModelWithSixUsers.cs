using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class ChangeUserViewModelWithSixUsers : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }

        public ChangeUserViewModelWithSixUsers(NavigationStore navigationStore)
        {
           //NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
        }
    }
}
