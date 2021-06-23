using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TypeIT.Models;

namespace TypeIT.Utilities
{
    public static class XmlHandler
    {
        /// <summary>
        /// when a new user comes into the application, a new .TypeIT file is created.
        /// </summary>
        /// <param name="name"></param>
        public static void NewUser(string name)
        {
            XDocument doc;
            doc = new XDocument(new XElement("UserProfile",
                new XElement("Name", name), //after specifying the tag, the value should be added
                new XElement("Statistics",
                    new XElement("HighestWPM"),
                    new XElement("AverageWPM"),
                    new XElement("AverageAccuracy"),
                    new XElement("HoursSpent"),
                    new XElement("TypedWordsTotal"),
                    new XElement("DailyRecords")),
                new XElement("Settings",
                    new XElement("Theme"),
                    new XElement("GameMode")),
                new XElement("Achievements"),
                new XElement("Documents")));

            doc.Root.Element("Statistics").Element("HighestWPM").Value = "0";
            doc.Root.Element("Statistics").Element("AverageWPM").Value = "0";
            doc.Root.Element("Statistics").Element("AverageAccuracy").Value = "0";
            doc.Root.Element("Statistics").Element("HoursSpent").Value = "0";
            doc.Root.Element("Statistics").Element("TypedWordsTotal").Value = "0";

            doc.Save("../../../FileTypes/Users/" + name + ".TypeIT");
        }

        /// <summary>
        /// Getting the elements of the tags. 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static List<string> GetElementsFromTags(string filePath, string tag)
        {
            List<string> listElements = new List<string>();

            XDocument doc = XDocument.Load(filePath);

            foreach (XElement element in doc.Descendants(tag))
            {
                string elementValue = (string) element;
                listElements.Add(elementValue);
            }

            return listElements;
        }

        /// <summary>
        ///  Method to update specific elements from the update settings
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        public static void UpdateUserStatistics(string userName, string tag, string value)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            int pageCount = 0;
            XmlHandler.GetElementsFromTags(filePath, "UserPageNumber")
                .ForEach(x => pageCount += int.Parse(x) <= 0 ? 0 : int.Parse(x));

            int avg = int.Parse(XmlHandler.GetElementsFromTags(filePath, tag).FirstOrDefault() ?? "0");

            avg = (int) ((avg * (pageCount - 1) + Double.Parse(value, CultureInfo.InvariantCulture)) / pageCount);

            XElement statistics = doc.Root?.Elements("Statistics").FirstOrDefault();

            if (statistics != null)
            {
                XElement docAverage = statistics.Element(tag);

                if (docAverage != null)
                {
                    docAverage.Value = avg.ToString();
                }
            }

            doc.Save(filePath);
        }

        /// <summary>
        /// Method to delete a document
        /// </summary>
        /// <param name="userName"></param>+
        public static void DeleteDocument(string userName, string documentName)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);
            //doc.Root.Element("Documents").Element("Document").Element(documentName).Remove();

            XElement documents = doc.Root.Element("Documents");

            XElement document = documents.Elements("Document")
                .Where(x => (string) x.Element("DocumentName") == documentName).SingleOrDefault();

            document.Remove();

            doc.Save(filePath);
        }

        /// <summary>
        /// Adds a new daily record to the user.
        /// If the record already exists for a given day the record is only updated if the new WPM is higher than the existing.
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <param name="date">The date when he set the record. yyyy-MM-dd</param>
        /// <param name="wpm">The new WPM</param>
        /// <param name="accuracy">The new accuracy</param>
        public static void AddDailyRecordToUser(string userName, string date, int wpm, double accuracy)
        {          
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";

            XDocument doc = XDocument.Load(filePath);

            // Load the daily record into the day variable
            XElement dailyRecords = doc.Root.Element("Statistics").Element("DailyRecords");
            XElement day = dailyRecords.Elements("Day")
                        .SingleOrDefault(x => (string)x.Element("Date")?.Value == date);

            // If day is null the record does not exsist, we create a new one
            if (day == null)
            {
                day = new XElement("Day",
                                        new XElement("Date", date),
                                        new XElement("WPM", wpm),
                                        new XElement("Average", accuracy));

                doc.Root.Element("Statistics").Element("DailyRecords").Add(day);
            }
            // If the records exists already we have to check if the current performance is higher than today's highest
            else
            {
                // Getting the existing values from the .TypeIT
                int existingWpm = int.Parse(day.Element("WPM")?.Value);

                if (wpm > existingWpm)
                {
                    int newWpm = wpm;
                    double newAccuracy = accuracy;

                    // Adding
                    day.Element("WPM").Value = newWpm.ToString();
                    day.Element("Average").Value = newAccuracy.ToString();
                }                
            }
                     
            doc.Save(filePath);
        }

        public static double GetBookAccuracy(string userName, string bookTitle)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";

            XDocument doc = XDocument.Load(filePath);

            // Load the daily record into the day variable
            XElement documents = doc.Root.Element("Documents");
            XElement book = documents.Elements("Document")
                        .SingleOrDefault(x => (string)x.Element("DocumentName")?.Value == bookTitle);

            return double.Parse(book.Element("DocumentAccuracy").Value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the number of books the user has completely finished.
        /// </summary>
        /// <param name="userName">The user's documents to be checked</param>
        /// <returns>The number of finished books. integer</returns>
        public static int GetUserBooksFinished(string userName)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            int documentsFinished = 0;

            List<string> totalPageNumber = XmlHandler.GetElementsFromTags(filePath, "TotalPageNumber");
            List<string> userPageNumber = XmlHandler.GetElementsFromTags(filePath, "UserPageNumber");

            for (int i = 0; i < totalPageNumber.Count; i++)
            {
                // If the user's page number equals to the total page number the book is finished
                if (int.Parse(totalPageNumber[i]) == int.Parse(userPageNumber[i]))
                {
                    documentsFinished++;
                }
            }

            return documentsFinished;
        }

        /// <summary>
        /// Method to update the elements from settings
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="userName"></param>
        /// <param name="tags"></param>
        /// <param name="mode"></param>
        public static void UpdateSettings(string userName, string tags, string mode)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";

            XDocument doc = XDocument.Load(filePath);

            doc.Root.Element("Settings").Element(tags).Value = mode;

            doc.Save(filePath);
        }

        public static void AddAchievementToUser(string userName, string achievementName)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";

            XDocument doc = XDocument.Load(filePath);
            XElement achievement = new XElement("AchievementName", achievementName);

            doc.Root.Element("Achievements").Add(achievement);

            doc.Save(filePath);
        }

        /// <summary>
        /// method to update elements from achievements
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tags"></param>
        /// <param name="value"></param>
        public static void UpdateAchiemvents(string filePath, string tags, string value)
        {
            XDocument doc = XDocument.Load(filePath);

            doc.Root.Element("Achievements").Element("Achievement").Element(tags).Value = value;

            doc.Save(filePath);
        }

        /// <summary>
        /// Method to update documents elements
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="docName"></param>
        /// <param name="userPage"></param>
        /// <param name="docAccuracy"></param>
        public static void UpdateDocuments(string userName, string docName, string userPage, string docAccuracy)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            XElement documents = doc.Root?.Element("Documents");

            if (documents != null)
            {
                XElement document = documents.Elements("Document")
                    .SingleOrDefault(x => (string) x.Element("DocumentName") == docName);

                if (document != null)
                {
                    double accuracy =
                        (double.Parse(document.Element("UserPageNumber")?.Value ?? string.Empty) *
                         double.Parse(document.Element("DocumentAccuracy")?.Value ?? string.Empty) +
                         double.Parse(docAccuracy)) /
                        double.Parse(userPage);
                }

                if (document != null)
                {
                    document.Element("UserPageNumber").Value = userPage;
                    document.Element("DocumentAccuracy").Value = docAccuracy;
                }
            }

            doc.Save(filePath);
        }

        public static DocumentModel GetRandomUserDocument(string user)
        {
            DocumentModel model = null;

            string filePath = $"../../../FileTypes/Users/{user}.TypeIT";

            XDocument doc = XDocument.Load(filePath);

            if (doc.Root != null)
            {
                XElement documents = doc.Root.Element("Documents");

                Random rng = new Random();

                if (documents != null)
                {
                    int count = documents.Elements("Document").Count();

                    XElement document = documents.Elements("Document").Skip(rng.Next(0, count)).FirstOrDefault();

                    if (document != null &&
                        Directory.Exists($"../../../Documents/{document.Element("DocumentName")?.Value}"))
                    {
                        model = new DocumentModel($"../../../Documents/{document.Element("DocumentName")?.Value}")
                        {
                            UserPageNumber = int.Parse(document.Element("UserPageNumber")?.Value ?? "0")
                        };
                    }
                }
            }

            return model;
        }

        public static DocumentModel GetUserDocument(string user, string docName)
        {
            string docPath = $"../../../Documents/{docName}";
            DocumentModel model = null;

            string filePath = $"../../../FileTypes/Users/{user}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            if (doc.Root != null)
            {
                XElement documents = doc.Root.Element("Documents");

                if (documents != null)
                {
                    XElement document = documents.Elements("Document")
                        .SingleOrDefault(x => (string) x.Element("DocumentName")?.Value == docName);

                    if (document != null && Directory.Exists(docPath))
                    {
                        model = new DocumentModel(docPath)
                        {
                            UserPageNumber = int.Parse(document.Element("UserPageNumber")?.Value ?? string.Empty)
                        };
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Adding a document into the users .TypeIT file when a new document is added.  
        /// </summary>
        /// <param name="userName"></param>
        public static void AddingADocumentIntoUserXml(string userName, string documentName, int documentNumberOfPages,
            int currentPage)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            XElement documents = doc.Root.Element("Documents");

            XElement existing = documents.Elements("Document")
                .SingleOrDefault(x => (string) x.Element("DocumentName") == documentName);

            if (existing == null)
            {
                XElement document = new XElement("Document");
                XElement name = new XElement("DocumentName");
                XElement numberTotal = new XElement("TotalPageNumber");
                XElement userNumber = new XElement("UserPageNumber");
                XElement docAccuracy = new XElement("DocumentAccuracy");

                name.Value = documentName;
                numberTotal.Value = documentNumberOfPages.ToString();
                userNumber.Value = currentPage.ToString();
                docAccuracy.Value = "0";

                document.Add(name);
                document.Add(numberTotal);
                document.Add(userNumber);
                document.Add(docAccuracy);

                documents.Add(document);
                doc.Save(filePath);
            }
        }

        /// <summary>
        /// Clears the statistics of the name provided
        /// </summary>
        /// <param name="userName"></param>
        public static void ClearUserData(string userName)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            foreach (XNode node in doc.Descendants("Statistics").DescendantNodes())
            {
                ((XElement) node).Value = "";
            }

            foreach (XNode node in doc.Descendants("DailyRecords").DescendantNodes())
            {
                ((XElement) node).Value = "";
            }

            foreach (XElement node in doc.Descendants("Documents"))
            {
                node.Value = "";
            }

            foreach (XElement node in doc.Descendants("Achievements"))
            {
                node.Value = "";
            }

            doc.Root.Element("Statistics").Element("HighestWPM").Value = "0";
            doc.Root.Element("Statistics").Element("AverageWPM").Value = "0";
            if (doc.Root != null)
            {
                doc.Root.Element("Statistics").Element("AverageAccuracy").Value = "0";
                doc.Root.Element("Statistics").Element("HoursSpent").Value = "0";
                doc.Root.Element("Statistics").Element("TypedWordsTotal").Value = "0";
            }

            doc.Save(filePath);
        }

        /// <summary>
        /// Gets all the data for the charts in the statistcs page
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static object[] GetValues(string userName)
        {
            object[] Values = new object[3];
            ChartValues<int> WpmValues = new ChartValues<int>();
            ChartValues<double> AccuracyValues = new ChartValues<double>();
            ObservableCollection<string> Dates = new ObservableCollection<string>();

            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            foreach (XElement Days in doc.Descendants("DailyRecords"))
            {
                foreach (XElement Day in Days.Descendants("Day"))
                {
                    WpmValues.Add(int.Parse(Day.Element("WPM").Value));

                    AccuracyValues.Add(double.Parse(Day.Element("Average").Value));

                    Dates.Add(DateTime.Parse(Day.Element("Date").Value).ToString("yyyy-MM-dd"));
                }
            }

            Values[0] = WpmValues;
            Values[1] = AccuracyValues;
            Values[2] = Dates;

            return Values;
        }
    }
}