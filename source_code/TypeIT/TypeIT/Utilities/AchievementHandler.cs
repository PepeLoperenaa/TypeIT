using System.Collections.Generic;
using System.Windows;
using TypeIT.Stores;

namespace TypeIT.Utilities
{
    public static class AchievementHandler
    {
        private static List<string> NewlyUnlockedAchievements = new List<string>();

        /// <summary>
        /// This method should be called when the user finishes a page. If he manages to successfully unlock an achievement it will be added to his .TypeIT file.
        /// </summary>
        /// <param name="currentUser">The current user. It is needed to see his already unlocked achievements and his username.</param>
        /// <param name="wpm">The WPM to be compared to the achievements requirements.</param>
        /// <param name="accuracy">The accuracy to be compared to the achievements requirements.</param>
        public static void FinishPageAchievements(UserStore currentUser, int wpm, double accuracy)
        {
            NewlyUnlockedAchievements.Clear();

            List<string> achievementName = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "AchievementName");
            List<string> achievementThreshold = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Treshold");
            List<string> achievementProperty = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Property");

            for (int i = 0; i < achievementName.Count; i++)
            {
                switch (achievementProperty[i])
                {
                    case "WPM":
                        if (wpm >= int.Parse(achievementThreshold[i]))
                        {
                            NewlyUnlockedAchievements.Add(achievementName[i]);
                        }
                        break;
                    case "Acc":
                        if (accuracy >= double.Parse(achievementThreshold[i]))
                        {
                            NewlyUnlockedAchievements.Add(achievementName[i]);
                        }
                        break;
                }
            }

            AddNewAchievements(currentUser);
        }

        /// <summary>
        /// Adds the achievements to the user's .TypeIT file
        /// </summary>
        /// <param name="currentUser">The current user that unlocked the achievements.</param>
        private static void AddNewAchievements(UserStore currentUser)
        {
            for (int i = 0; i < NewlyUnlockedAchievements.Count; i++)
            {
                if (!currentUser.CurrentUser.Achievements.Contains(NewlyUnlockedAchievements[i]))
                {
                    XmlHandler.AddAchievementToUser(currentUser.CurrentUser.Name, NewlyUnlockedAchievements[i]);

                    //Custom messagebox
                    var res = Xceed.Wpf.Toolkit.MessageBox.Show(
                                   "You have unlocked: " + NewlyUnlockedAchievements[i],
                                   "Congratulations!",
                                   MessageBoxButton.OK
                               );
                }
            }
        }
    }
}
