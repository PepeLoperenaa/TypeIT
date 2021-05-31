﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class ChangeGameModeViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand ChangeGameModeToCasual { get; set; }
        public ICommand ChangeGameModeToNormal { get; set; }
        public ICommand ChangeGameModeToHard { get; set; }
        public ICommand ChangeGameModeToExtreme { get; set; }

        public ChangeGameModeViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
            //ChangeGameModeToCasual
            //ChangeGameModeToNormal
            //ChangeGameModeToHard
            //ChangeGameModeToExtreme
        }
    }
}