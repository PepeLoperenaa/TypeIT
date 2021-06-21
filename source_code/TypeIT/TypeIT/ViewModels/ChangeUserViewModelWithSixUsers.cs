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
using TypeIT.Models;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class ChangeUserViewModelWithSixUsers : ChangeUserViewModel
    {
        public ChangeUserViewModelWithSixUsers(NavigationStore navigationStore): base(navigationStore)
        {}
    }
}
