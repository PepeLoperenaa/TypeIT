﻿using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.FileTypes;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    class ChangeGameModeViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateSettingsCommand { get; }
        public ICommand ChangeGameModeToCasual { get; set; }
        public ICommand ChangeGameModeToNormal { get; set; }
        public ICommand ChangeGameModeToHard { get; set; }
        public ICommand ChangeGameModeToExtreme { get; set; }
        public UserStore currentUser { get; set; }

        public ChangeGameModeViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationStore, () => new SettingsViewModel(navigationStore, userStore));
            currentUser = userStore;

            ChangeGameModeToCasual = new DelegateCommand(setToCasual);
            ChangeGameModeToNormal = new DelegateCommand(setToNormal);
            ChangeGameModeToHard = new DelegateCommand(setToHard);
            ChangeGameModeToExtreme = new DelegateCommand(setToExtreme);
        }

        private void setToCasual()
        {
            XmlHandler.updateSettings(currentUser.CurrentUser.Name, "GameMode", "Casual");
            currentUser.CurrentUser.GameMode = Difficulty.Easy;
            NavigateSettingsCommand.Execute(null);
        }

        private void setToNormal()
        {
            XmlHandler.updateSettings(currentUser.CurrentUser.Name, "GameMode", "Normal");
            currentUser.CurrentUser.GameMode = Difficulty.Medium;
            NavigateSettingsCommand.Execute(null);
        }

        private void setToHard()
        {
            XmlHandler.updateSettings(currentUser.CurrentUser.Name, "GameMode", "Hard");
            currentUser.CurrentUser.GameMode = Difficulty.Hard;
            NavigateSettingsCommand.Execute(null);
        }

        private void setToExtreme()
        {
            XmlHandler.updateSettings(currentUser.CurrentUser.Name, "GameMode", "Extreme");
            currentUser.CurrentUser.GameMode = Difficulty.Extreme;
            NavigateSettingsCommand.Execute(null);
        }
    }
}
