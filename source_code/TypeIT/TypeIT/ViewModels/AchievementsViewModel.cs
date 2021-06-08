using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using TypeIT.Commands;
using TypeIT.FileTypes;
using TypeIT.Models;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class AchievementsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public UserStore currentUser { get; set; }
        public List<AchievementModel> Achievements { get; set; }

        public AchievementsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            Achievements = new List<AchievementModel>();
            currentUser = userStore;
            loadAchievements();
        }

        private void loadAchievements()
        {
            List<string> achievementName = XmlHandler.getElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "AchievementName");
            List<string> achievementDesc = XmlHandler.getElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Description");

            //Adding the achievements to the field that is binded to the view
            for (int i = 0; i < achievementName.Count; i++)
            {
                AchievementModel achievementModel = new AchievementModel(achievementName[i], achievementDesc[i]);
                Achievements.Add(achievementModel);
            }
        }
    }
}
