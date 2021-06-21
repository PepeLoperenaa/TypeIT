﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using TypeIT.Models;

namespace TypeIT.FileTypes
{
    public static class XmlHandler
    {
        /// <summary>
        /// when a new user comes into the application, a new .TypeIT file is created.
        /// </summary>
        /// <param name="name"></param>
        public static void newUser(string name)
        {
            XDocument doc;
            doc = new XDocument(new XElement("UserProfile",
                                       new XElement("Name", name), //after specifying the tag, the value should be added
                                       new XElement("Statistics",
                                           new XElement("HighestWPM"),
                                           new XElement("AverageWPM"),
                                           new XElement("AverageAccuracy"),
                                           new XElement("HoursSpent"),
                                           new XElement("TypedTypedWordsTotalWords"),
                                           new XElement("DailyRecords",
                                               new XElement("Day",
                                               new XElement("Date"),
                                               new XElement("WPM"),
                                               new XElement("Average")))),
                                       new XElement("Settings",
                                           new XElement("Theme"),
                                           new XElement("GameMode")),
                                       new XElement("Achievements",
                                           new XElement("Achievement",
                                               new XElement("AchievementName"))),
                                       new XElement("Documents")));

            doc.Save("../../../FileTypes/Users/" + name + ".TypeIT");
        }

        /// <summary>
        /// Getting the elements of the tags. 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static List<string> getElementsFromTags(string filePath, string tag)
        {
            List<string> listElements = new List<string>();

            XDocument doc = XDocument.Load(filePath);

            foreach (XElement element in doc.Descendants(tag))
            {
                string elementValue = (string)element;
                listElements.Add(elementValue);
            }
            return listElements;
        }

        /// <summary>
        /// Method to add data to the different .TypeIT files
        /// </summary>
        /// <param name="filePath"></param>
        public static void AddingDataIntoAnXml(string filePath) 
        {
            XDocument doc = XDocument.Load(filePath);

            doc.Root.Element("Name").Value = "example ";
            doc.Root.Element("Statistics").Element("HighestWPM").Value = "0";
            doc.Root.Element("Statistics").Element("AverageWPM").Value = "0";
            doc.Root.Element("Statistics").Element("AverageAccuracy").Value = "0";
            doc.Root.Element("Statistics").Element("HoursSpent").Value = "0";
            doc.Root.Element("Statistics").Element("TypedWordsTotal").Value = "0";
            doc.Root.Element("Statistics").Element("DailyRecords").Element("Day").Element("Date").Value = "0";
            doc.Root.Element("Statistics").Element("DailyRecords").Element("Day").Element("WPM").Value = "0";
            doc.Root.Element("Statistics").Element("DailyRecords").Element("Day").Element("Average").Value = "0";
            doc.Root.Element("Statistics").Element("DailyRecords").Element("Day").Element("Accuracy").Value = "0";
            doc.Root.Element("Settings").Element("Theme").Value = "LightMode";
            doc.Root.Element("Settings").Element("GameMode").Value = "Hard";
            doc.Root.Element("Achievements").Element("Achievement").Element("AchievementName").Value = "Example";
            doc.Root.Element("Documents").Element("Document").Element("DocumentName").Value = "Example";
            doc.Root.Element("Documents").Element("Document").Element("TotalPageNumber").Value = "Example";
            doc.Root.Element("Documents").Element("Document").Element("UserPageNumber").Value = "Example";
            doc.Root.Element("Documents").Element("Document").Element("DocumentAccuracy").Value = "Example";

            doc.Save(filePath);
        }

        /// <summary>
        ///  Method to update specific elements from the update settings
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public static void UpdateStatistics(string userName, string type, string value)
        {
            XDocument doc = XDocument.Load($"../../../FileTypes/Users/{userName}.TypeIT");

            doc.Root.Element("Statistics").Element(type).Value = value;
        }

        /// <summary>
        /// Method to delete a document
        /// </summary>
        /// <param name="userName"></param>+
        public static void DeleteDocument(string userName, string documentName)
        {
            XDocument doc = XDocument.Load($"../../../FileTypes/Users/{userName}.TypeIT");
            doc.Root.Element("Documents").Element("Document").Element(documentName).Remove();
        }

        /// <summary>
        /// Method to update the elements for daily records
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="element"></param>
        /// <param name="daily"></param>
        /// <param name="value"></param>
        public static void UpdateDailyRecords(string userName, string element, string daily , string value)
        {
            XDocument doc = XDocument.Load($"../../../FileTypes/Users/{userName}.TypeIT");

            doc.Root.Element("Statistics").Element("DailyRecords").Element(element).Element(daily).Value = value;
        }

        /// <summary>
        /// Method to update the elements from settings
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tags"></param>
        /// <param name="mode"></param>
        public static void updateSettings(string userName, string tags, string mode)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";

            XDocument doc = XDocument.Load(filePath);

            doc.Root.Element("Settings").Element(tags).Value = mode;

            doc.Save(filePath);

        }

        /// <summary>
        /// method to update elements from achievements
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tags"></param>
        /// <param name="value"></param>
        public static void updateAchiemvents(string filePath, string tags, string value)
        {
            XDocument doc = XDocument.Load(filePath);

            doc.Root.Element("Achievements").Element("Achievement").Element(tags).Value = value;

            doc.Save(filePath);
        }

        /// <summary>
        /// Method to update documents elements
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        public static void updateDocuments(string userName, string docName, string userPage, string docAccuracy)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            XElement documents = doc.Root.Element("Documents");

            XElement document = documents.Elements("Document").Where(x => (string)x.Element("DocumentName") == docName).SingleOrDefault();

            document.Element("UserPageNumber").Value = userPage;
            document.Element("DocumentAccuracy").Value = docAccuracy;

            doc.Save(filePath);
        }

        public static DocumentModel GetUserDocument(string user, string docPath)
        {
            DocumentModel model = new DocumentModel(docPath);

            string filePath = $"../../../FileTypes/Users/{user}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            XElement documents = doc.Root.Element("Documents");

            XElement document = documents.Elements("Document").Where(x => (string)x.Element("DocumentName") == model.Name).SingleOrDefault();

            model.UserPageNumber = Int32.Parse(document.Element("UserPageNumber").Value);

            return model;
        }

        /// <summary>
        /// Adding a document into the users .TypeIT file when a new document is added.  
        /// </summary>
        /// <param name="userName"></param>
        public static void AddingADocumentIntoUserXml(string userName, string documentName, int documentNumberOfPages, int currentPage)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            XElement documents = doc.Root.Element("Documents");

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

        /// <summary>
        /// Clears the statistics of the name provided
        /// </summary>
        /// <param name="userName"></param>
        public static void ClearUserStatistics(string userName)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            foreach (XNode node in doc.Descendants("Statistics").DescendantNodes())
            {
                ((XElement)node).Value = "";
            }
            doc.Save(filePath);
        }

        // remove document from user
        // get documentmodel from user

        public static void unlockAchievements(string userName, string AchievementName)
        {
            string filePath = $"../../../FileTypes/Users/{userName}.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            foreach (XNode node in doc.Descendants("Achievements").Descendants(AchievementName).DescendantNodes())
            {
                ((XElement)node).Value = AchievementName;
            }
            doc.Save(filePath);
        }
    }
}