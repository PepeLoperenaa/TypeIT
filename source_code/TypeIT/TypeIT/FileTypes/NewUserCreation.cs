﻿using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

namespace TypeIT.FileTypes
{
    public static class NewUserCreation
    {
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
                                       new XElement("Documents",
                                           new XElement("Document", 
                                                new XElement("DocumentName"), 
                                                new XElement("TotalPageNumber"), 
                                                new XElement("UserPageNumber"), 
                                                new XElement("DocumentAccuracy")))));

            doc.Save("../../../FileTypes/Users/" + name + ".TypeIT");
            //when we save in the current directory, it will save the data in the debug folder.
        }

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
        public static void AddingDataIntoAnXml(string filePath) //to add information into the element
        {
            //string file = "C:/Users/MyPC/source/repos/XMLChanging/XMLChanging/folder/userFile.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            doc.Root.Element("Name").Value = "Example";
            doc.Root.Element("Statistics").Element("HighestWPM").Value = "20";
            doc.Root.Element("Statistics").Element("AverageWPM").Value = "20";
            doc.Root.Element("Statistics").Element("AverageAccuracy").Value = "20";
            doc.Root.Element("Statistics").Element("HoursSpent").Value = "20";
            doc.Root.Element("Statistics").Element("TypedTypedWordsTotalWords").Value = "20";
            doc.Root.Element("Settings").Element("Theme").Value = "LightMode";
            doc.Root.Element("Settings").Element("GameMode").Value = "Hard";
            doc.Root.Element("Achievements").Element("Achievement").Element("AchievementName").Value = "Yes";
            doc.Save(filePath);
        }
    }
}
