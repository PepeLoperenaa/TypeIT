using System.Collections.Generic;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    class UnlockedAchievementsViewModel
    {
        public void collectionAchievements(string userName)
        {
            List<string> achievements = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> document = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "Document");

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("You have to start somewhere") && document.Count == 1)
                {
                    XmlHandler.UnlockAchievements(userName, "You have to start somewhere");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Library owner") && document.Count == 10)
                {
                    XmlHandler.UnlockAchievements(userName, "Library owner");
                }
            }
        }


        public void finishPages(string userName)
        {
            List<string> achievements = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> total = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "TotalPageNumber");
            List<string> user = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "UserPageNumber");

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("First steps") && user.Equals(1))
                {
                    XmlHandler.UnlockAchievements(userName, "First steps");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("What book is that?") && total.Count == 1000 && user.Count <= 1000)
                {
                    XmlHandler.UnlockAchievements(userName, "What book is that?");
                }
            }

        }

        public void speedAchievements(string userName)
        {
            List<string> achievements = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> HighestWPM = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "HighestWPM");

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Fast as the wind") && HighestWPM.Equals("80"))
                {
                    XmlHandler.UnlockAchievements(userName, "Fast as the wind");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Faster than the wind") && HighestWPM.Equals("130"))
                {
                    XmlHandler.UnlockAchievements(userName, "Faster than the wind");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("No skill luck only") && HighestWPM.Equals("180"))
                {
                    XmlHandler.UnlockAchievements(userName, "No skill luck only");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Did I just do that ?") && HighestWPM.Equals("200"))
                {
                    XmlHandler.UnlockAchievements(userName, "Did I just do that ?");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("No skill luck only") && HighestWPM.Equals("300"))
                {
                    XmlHandler.UnlockAchievements(userName, "No skill luck only");
                }
            }
        }

        public void averageSpeedAchievements(string userName)
        {
            List<string> achievements = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> AverageWpm = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/Pepe.TypeIT", "AverageWPM");


            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("The birth of a legend") && AverageWpm.Equals("40"))
                {
                    XmlHandler.UnlockAchievements(userName, "The birth of a legend");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("You made it") && AverageWpm.Equals("80"))
                {
                    XmlHandler.UnlockAchievements(userName, "You made it");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Typing professional") && AverageWpm.Equals("90"))
                {
                    XmlHandler.UnlockAchievements(userName, "Typing professional");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Fast and furious") && AverageWpm.Equals("100"))
                {
                    XmlHandler.UnlockAchievements(userName, "Fast and furious");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Beat the creator") && AverageWpm.Equals("140"))
                {
                    XmlHandler.UnlockAchievements(userName, "Beat the creator");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Quit this game") && AverageWpm.Equals("170"))
                {
                    XmlHandler.UnlockAchievements(userName, "Quit this game");
                }
            }

            if (AverageWpm.Equals("170"))
            {
                XmlHandler.UnlockAchievements("Pepe", "Quit this game");
            }
        }

        public void mistakesAchievements(string userName)
        {
            List<string> achievements = XmlHandler.GetElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> acc = XmlHandler.GetElementsFromTags("../../../FileTypes/TypeitFiles/Pepe.TypeIT", "DocumentAccuracy");

            for (int i = 0; i < achievements.Count; i++)
            {
                if (achievements.Contains("You cheated to get this didn't you") && acc.Equals("100"))
                {
                    XmlHandler.UnlockAchievements(userName, "You cheated to get this didn't you");
                }
            }
        }
    }
}
