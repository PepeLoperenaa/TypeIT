using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public ICommand ContactCommand { get; set; }
        public UserStore currentUser { get; set; }

        public AboutViewModel(NavigationStore navigationStore, UserStore userStore)
        {
           NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
           currentUser = userStore;

            ContactCommand = new DelegateCommand(ClickedContactUs);
        }

        private void ClickedContactUs()
        {
            var url = "mailto:rob.smit@nhlstenden.com?subject=TypeIT%20Contact%20us%20form&amp";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
    }
}
