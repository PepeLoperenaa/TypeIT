using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class AboutViewModel : ViewModelBase
    {
        public ICommand NavigateLoginCommand { get; }

        public AboutViewModel(NavigationStore navigationStore)
        {
           
        }
    }
}
