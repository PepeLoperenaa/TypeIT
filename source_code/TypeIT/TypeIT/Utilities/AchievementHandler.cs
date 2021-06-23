using System.Collections.Generic;
using System.Windows;
using TypeIT.Stores;

namespace TypeIT.Utilities
{
    public static class AchievementHandler
    {
        private static List<string> NewlyUnlockedAchievements = new List<string>();

        /// <summary>
        /// This method should be called when the user finishes a page.
        /// If he manages to successfully unlock an achievement it will be added to his .TypeIT file.
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
                    // WPM unlocks when you reach a certain Words per minute value per page
                    case "WPM":
                        if (wpm >= int.Parse(achievementThreshold[i]))
                        {
                            NewlyUnlockedAchievements.Add(achievementName[i]);
                        }
                        break;
                    // Acc unlocks when you reach a certain accuracy per page
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
        /// This method should be called when the user adds a book
        /// If he manages to successfully unlock an achievement it will be added to his .TypeIT file.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="documentsNumber"></param>
        public static void AddBookAchievements(UserStore currentUser, int documentsNumber)
        {
            NewlyUnlockedAchievements.Clear();

            List<string> achievementName = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "AchievementName");
            List<string> achievementThreshold = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Treshold");
            List<string> achievementProperty = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Property");

            for (int i = 0; i < achievementName.Count; i++)
            {
                switch (achievementProperty[i])
                {
                    // BookAdd unlocks when you add a specific number of books
                    case "BookAdd":
                        if (documentsNumber >= int.Parse(achievementThreshold[i]))
                        {
                            NewlyUnlockedAchievements.Add(achievementName[i]);
                        }
                        break;
                }
            }

            AddNewAchievements(currentUser);
        }

        /// <summary>
        /// Unlocks methods associated with finishing a book.
        /// Should be called when the user finishes a book
        /// </summary>
        /// <param name="currentUser">The current user playing</param>
        /// <param name="pageNumber">The number of pages of the finished book</param>
        /// <param name="documentName">The document that was finished</param>
        public static void FinishBookAchievements(UserStore currentUser, int pageNumber, string documentName)
        {
            NewlyUnlockedAchievements.Clear();

            List<string> achievementName = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "AchievementName");
            List<string> achievementThreshold = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Treshold");
            List<string> achievementProperty = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/achievements.TypeIT", "Property");

            for (int i = 0; i < achievementName.Count; i++)
            {
                switch (achievementProperty[i])
                {
                    // Page achievements unlock with books of a specific length
                    case "Page":
                        if (pageNumber >= int.Parse(achievementThreshold[i]))
                        {
                            NewlyUnlockedAchievements.Add(achievementName[i]);
                        }
                        break;
                    // BookFinish achievements unlock with a specific number of finished books
                    case "BookFinish":
                        if (XmlHandler.GetUserBooksFinished(currentUser.CurrentUser.Name) >= int.Parse(achievementThreshold[i]))
                        {
                            NewlyUnlockedAchievements.Add(achievementName[i]);
                        }
                        break;
                    // BookAcc unlocks with a specific book accuracy
                    case "BookAcc":
                        if (XmlHandler.GetBookAccuracy(currentUser.CurrentUser.Name, documentName) >= int.Parse(achievementThreshold[i]))
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
        /// Based on the local field called 'NewlyUnlockedAchievements'
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
