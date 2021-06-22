using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TypeIT.Utilities;

namespace TypeIT.Models
{
    public class UserModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string Theme { get; set; }
        public Difficulty GameMode { get; set; }
        public ObservableCollection<string> Achievements { get; set; }

        private ObservableCollection<DocumentModel> _document;
        private StatisticsModel _statistics;

        public ObservableCollection<DocumentModel> Documents
        {
            get => _document;
            set
            {
                _document = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Documents)));
            }
        }
        public StatisticsModel Statistics
        {
            get => _statistics;
            set
            {
                _statistics = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Statistics)));
            }
        }

        public UserModel(string name, bool load)
        {
            Name = name;
            Statistics = LoadStatistics();
            Achievements = new ObservableCollection<string>();
            Documents = new ObservableCollection<DocumentModel>();
            Theme = LoadTheme();

            LoadUsersAchievements();
            LoadUserDocuments();
            LoadStatistics();
            LoadUserGameMode();
        }

        public void RefreshUserModel()
        {
            LoadUsersAchievements();
            LoadUserDocuments();
            Statistics = LoadStatistics();
            LoadUserGameMode();
        }

        /// <summary>
        /// Loads the user's unlocked achievements from his .TypeIT file
        /// The readings are loaded into the Achievements List
        /// </summary>
        private void LoadUsersAchievements()
        {
            Achievements.Clear();

            // Load unlocked achievements
            List<string> hallo = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "AchievementName");
            foreach (string achievementName in hallo)
            {
                Achievements.Add(achievementName);
            }
        }

        /// <summary>
        /// Loads the user's docuements from his .TypeIT file
        /// A Documentmodel object is created for each document
        /// The objects are added to the Documents List
        /// </summary>
        public void LoadUserDocuments()
        {
            Documents.Clear();

            //Reading the values from the .TpyeIT file
            List<string> ListDocumentName = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "DocumentName");
            List<string> ListTotalPageNumber = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "TotalPageNumber");
            List<string> ListUserPageNumber = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "UserPageNumber");
            List<string> ListDocumentAccuracy = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "DocumentAccuracy");

            //Loop through each list
            for (int i = 0; i < ListDocumentName.Count; i++)
            {
                //If the documentname is empty skip this entry
                //Needed because the reader can be buggy
                if (ListDocumentName[i] == "")
                {
                    break;
                }

                //Converting strings to numbers
                int totalPageNumber = Convert.ToInt32(ListTotalPageNumber[i]);
                int userPageNumber = Convert.ToInt32(ListUserPageNumber[i]);
                double accuracy = Convert.ToDouble(ListDocumentAccuracy[i]);

                //Create a new DocumentModel and add it to the Documents list
                DocumentModel document = new DocumentModel(ListDocumentName[i], totalPageNumber, userPageNumber, accuracy);
                Documents.Add(document);
            }
        }

        /// <summary>
        /// Loads the user's statistics from his .TypeIT file
        /// </summary>
        /// <returns>Returns a StatisticsModel with the user's statistics</returns>
        public StatisticsModel LoadStatistics()
        {
            //Reading Highest WPM and converting to double
            string parameter = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "HighestWPM").First();
            double HighestWPM = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            //Reading Average WPM and converting to int
            parameter = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "AverageWPM").First();
            int AverageWPM = (parameter == "") ? 0 : int.Parse(parameter);

            //Reading Average Accuracy and converting to double
            parameter = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "AverageAccuracy").First();
            double AverageAccuracy = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            //Reading Hour Spent and converting to double
            parameter = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "HoursSpent").First();
            double HoursSpent = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            //Reading Total words typed and converting to int
            parameter = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "TypedWordsTotal").First();
            int TypedTypedWordsTotalWords = (parameter == "") ? 0 : Convert.ToInt32(parameter);

            //Returning a new statistics model based on the read values
            return new StatisticsModel(HighestWPM, AverageWPM, AverageAccuracy, HoursSpent, TypedTypedWordsTotalWords);
        }

        /// <summary>
        /// Loads the user's theme from his .TypeIT file
        /// </summary>
        /// <returns>Returns the theme as string</returns>
        private string LoadTheme()
        {
            return XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "Theme").First();
        }

        /// <summary>
        /// Loads the user's selected game mode from his .TypeIT file
        /// Sets the enum value based on the game mode
        /// If the value is invalid it's set to easy
        /// </summary>
        private void LoadUserGameMode()
        {
            string gameMode = XmlHandler.GetElementsFromTags("../../../FileTypes/Users/" + Name + ".TypeIT", "GameMode").First();

            switch (gameMode)
            {
                case "Casual":
                    GameMode = Difficulty.Easy;
                    break;
                case "Normal":
                    GameMode = Difficulty.Medium;
                    break;
                case "Hard":
                    GameMode = Difficulty.Hard;
                    break;
                case "Extreme":
                    GameMode = Difficulty.Extreme;
                    break;
                default:
                    GameMode = Difficulty.Easy;
                    break;
            }
        }
    }
}
