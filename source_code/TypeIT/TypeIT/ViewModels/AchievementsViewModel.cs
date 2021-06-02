using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class AchievementsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ObservableCollection<AchievementModel> Achievements { get; set; }

        public AchievementsViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
            Achievements = new ObservableCollection<AchievementModel>();

            AchievementModel achievementModel = new AchievementModel("Achievement1", "Hello this is the description for achievement1 please work.");
            AchievementModel achievementModel2 = new AchievementModel("Achievement2", "Hello this is the description for achievement2 please work.");
            AchievementModel achievementModel3 = new AchievementModel("Achievement3", "Hello this is the description for achievement4 please work.");

            Achievements.Add(achievementModel);
            Achievements.Add(achievementModel2);
            Achievements.Add(achievementModel3);
        }
    }
}
