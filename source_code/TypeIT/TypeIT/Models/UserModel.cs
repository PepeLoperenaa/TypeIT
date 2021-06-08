using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeIT.FileTypes;
using TypeIT.Utilities;

namespace TypeIT.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public StatisticsModel Statistics { get; set; }
        public string Theme { get; set; }
        public Difficulty GameMode { get; set; }
        public List<string> Achievements { get; set; }
        public List<DocumentModel> Documents { get; set; }

        public UserModel(string name, bool load)
        {
            Name = name;
            Statistics = loadStatistics();
            Achievements = new List<string>();
            Documents = new List<DocumentModel>();

            loadUsersAchievements();
            loadUserDocuments();
            loadStatistics();
        }

        private void loadUsersAchievements()
        {
            // Load unlocked achievements
            List<string> hallo = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "AchievementName");
            foreach (string achievementName in hallo)
            {
                Achievements.Add(achievementName);
            }            
        }

        private void loadUserDocuments()
        {
            List<string> ListDocumentName = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "DocumentName");
            List<string> ListTotalPageNumber = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "TotalPageNumber");
            List<string> ListUserPageNumber = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "UserPageNumber");
            List<string> ListDocumentAccuracy = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "DocumentAccuracy");

            for (int i = 0; i < ListDocumentName.Count; i++)
            {
                if (ListDocumentName[i] == "")
                    break;

                int pageNumber = Convert.ToInt32(ListUserPageNumber[i]);
                double accuracy = Convert.ToDouble(ListDocumentAccuracy[i]);

                DocumentModel document = new DocumentModel(ListDocumentName[i], pageNumber, accuracy);
                Documents.Add(document);
            }
        }

        private StatisticsModel loadStatistics()
        {
            string parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "HighestWPM").First();
            double HighestWPM = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "AverageWPM").First();
            int AverageWPM = (parameter == "") ? 0 : Int32.Parse(parameter);

            parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "AverageAccuracy").First();
            double AverageAccuracy = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "HoursSpent").First();
            double HoursSpent = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "TypedTypedWordsTotalWords").First();
            int TypedTypedWordsTotalWords = (parameter == "") ? 0 : Convert.ToInt32(parameter);

            return new StatisticsModel(HighestWPM, AverageWPM, AverageAccuracy, HoursSpent, TypedTypedWordsTotalWords);
        }
    }
}
