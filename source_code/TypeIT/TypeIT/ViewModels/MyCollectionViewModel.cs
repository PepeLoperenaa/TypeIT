using Microsoft.Win32;
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
    class MyCollectionViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }

        public MyCollectionViewModel(NavigationStore navigationStore)
        {
           NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));

        }
    }
}
