using System.Collections.Generic;
using TypeIT.FileTypes;

namespace TypeIT.ViewModels
{
    class UnlockedAchievementsViewModel
    {
        public void collectionAchievements(string userName)
        {
            List<string> achievements = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> document = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "Document");

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("You have to start somewhere") && document.Count == 1)
                {
                    XmlHandler.unlockAchievements(userName, "You have to start somewhere");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Library owner") && document.Count == 10)
                {
                    XmlHandler.unlockAchievements(userName, "Library owner");
                }
            }
        }


        public void finishPages(string userName)
        {
            List<string> achievements = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> total = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "TotalPageNumber");
            List<string> user = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "UserPageNumber");

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("First steps") && user.Equals(1))
                {
                    XmlHandler.unlockAchievements(userName, "First steps");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("What book is that?") && total.Count == 1000 && user.Count <= 1000)
                {
                    XmlHandler.unlockAchievements(userName, "What book is that?");
                }
            }

        }

        public void speedAchievements(string userName)
        {
            List<string> achievements = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> HighestWPM = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "HighestWPM");

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Fast as the wind") && HighestWPM.Equals("80"))
                {
                    XmlHandler.unlockAchievements(userName, "Fast as the wind");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Faster than the wind") && HighestWPM.Equals("130"))
                {
                    XmlHandler.unlockAchievements(userName, "Faster than the wind");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("No skill luck only") && HighestWPM.Equals("180"))
                {
                    XmlHandler.unlockAchievements(userName, "No skill luck only");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Did I just do that ?") && HighestWPM.Equals("200"))
                {
                    XmlHandler.unlockAchievements(userName, "Did I just do that ?");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("No skill luck only") && HighestWPM.Equals("300"))
                {
                    XmlHandler.unlockAchievements(userName, "No skill luck only");
                }
            }
        }

        public void averageSpeedAchievements(string userName)
        {
            List<string> achievements = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> AverageWpm = XmlHandler.getElementsFromTags("../../../FileTypes/TypeitFiles/Pepe.TypeIT", "AverageWPM");


            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("The birth of a legend") && AverageWpm.Equals("40"))
                {
                    XmlHandler.unlockAchievements(userName, "The birth of a legend");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("You made it") && AverageWpm.Equals("80"))
                {
                    XmlHandler.unlockAchievements(userName, "You made it");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Typing professional") && AverageWpm.Equals("90"))
                {
                    XmlHandler.unlockAchievements(userName, "Typing professional");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Fast and furious") && AverageWpm.Equals("100"))
                {
                    XmlHandler.unlockAchievements(userName, "Fast and furious");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Beat the creator") && AverageWpm.Equals("140"))
                {
                    XmlHandler.unlockAchievements(userName, "Beat the creator");
                }
            }

            for (int i = 0; i < achievements.Count; i++)
            {
                if (!achievements.Contains("Quit this game") && AverageWpm.Equals("170"))
                {
                    XmlHandler.unlockAchievements(userName, "Quit this game");
                }
            }

            if (AverageWpm.Equals("170"))
            {
                XmlHandler.unlockAchievements("Pepe", "Quit this game");
            }
        }

        public void mistakesAchievements(string userName)
        {
            List<string> achievements = XmlHandler.getElementsFromTags($"../../../FileTypes/Users/{userName}.TypeIT", "AchievementName");
            List<string> acc = XmlHandler.getElementsFromTags("../../../FileTypes/TypeitFiles/Pepe.TypeIT", "DocumentAccuracy");

            for (int i = 0; i < achievements.Count; i++)
            {
                if (achievements.Contains("You cheated to get this didn't you") && acc.Equals("100"))
                {
                    XmlHandler.unlockAchievements(userName, "You cheated to get this didn't you");
                }
            }
        }
    }
}
