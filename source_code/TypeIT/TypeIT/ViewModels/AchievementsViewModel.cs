using System.Collections.Generic;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    class AchievementsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public UserStore currentUser { get; set; }
        public List<AchievementModel> Achievements { get; set; }
        public List<AchievementModel> UnlockedAchievements { get; set; }

        public AchievementsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            Achievements = new List<AchievementModel>();
            UnlockedAchievements = new List<AchievementModel>();
            currentUser = userStore;
            loadAchievements();
            loadUsersAchievements();
        }

        /// <summary>
        /// Loading the user achievements 
        /// </summary>
        private void loadUsersAchievements()
        {
            // We have to iterate backwards to safely delete elements
            for (int i = Achievements.Count - 1; i >= 0; i--)
            {
                if (currentUser.CurrentUser.Achievements.Contains(Achievements[i].Title))
                {
                    UnlockedAchievements.Add(Achievements[i]);
                    Achievements.Remove(Achievements[i]);
                }
            }
        }

        /// <summary>
        /// Load all of the achievements into the application from the user's .TypeIT file
        /// </summary>
        private void loadAchievements()
        {
            //Loading the achievements from .TypeIT
            List<string> achievementName = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "AchievementName");
            List<string> achievementDesc = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Description");

            //Adding the achievements to the field that is binded to the view (Achievements)
            for (int i = 0; i < achievementName.Count; i++)
            {
                AchievementModel achievementModel = new AchievementModel(achievementName[i], achievementDesc[i]);
                Achievements.Add(achievementModel);
            }
        }
    }
}
