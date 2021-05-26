using Prism.Commands;
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
    class DashboardViewModel : ViewModelBase
    {
        public ICommand NavigateAboutCommand { get; }
        public ICommand ExitCommand { get; set; }

        public DashboardViewModel(NavigationStore navigationStore)
        {
            NavigateAboutCommand = new NavigateCommand<AboutViewModel>(navigationStore, () => new AboutViewModel(navigationStore));
            ExitCommand = new DelegateCommand(ClickedExit);
        }

        private void ClickedExit()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}