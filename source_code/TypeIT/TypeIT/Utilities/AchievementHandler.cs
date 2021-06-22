using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeIT.Stores;

namespace TypeIT.Utilities
{
    public static class AchievementHandler
    {
        private static List<string> newlyUnlockedAchievements = new List<string>();

        public static void FinishPageAchievements(UserStore currentUser, int wpm, double accuracy)
        {
            List<string> achievementName = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "AchievementName");
            List<string> achievementTreshold = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Treshold");
            List<string> achievementProperty = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Property");

            for (int i = 0; i < achievementName.Count; i++)
            {
                switch (achievementProperty[i])
                {
                    case "WPM":
                        if (wpm >= Int32.Parse(achievementTreshold[i]))
                        {
                            newlyUnlockedAchievements.Add(achievementName[i]);
                        }
                        break;
                    case "Acc":
                        if (accuracy >= Double.Parse(achievementTreshold[i]))
                        {
                            newlyUnlockedAchievements.Add(achievementName[i]);
                        }
                        break;
                }
            }

            AddNewAchievements(currentUser);
        }

        private static void AddNewAchievements(UserStore currentUser)
        {
            // We have to iterate backwards to safely delete elements
            for (int i = currentUser.CurrentUser.Achievements.Count - 1; i >= 0; i--)
            {
                if (!currentUser.CurrentUser.Achievements.Contains(newlyUnlockedAchievements[i]))
                {
                    XmlHandler.AddAchievementToUser(currentUser.CurrentUser.Name, newlyUnlockedAchievements[i]);
                }
            }
        }
    }
}
