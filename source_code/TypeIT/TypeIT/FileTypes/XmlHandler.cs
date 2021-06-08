using System.Collections.Generic;
using System.Xml.Linq;
using TypeIT.Models;
using TypeIT.Objects;

namespace TypeIT.FileTypes
{
    public static class XmlHandler
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
                                       new XElement("Documents")));

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

        public static void AddingADocumentIntoUserXml(string userName)
        {
            XDocument doc = XDocument.Load($"../../../FileTypes/Users/{userName}.TypeIT"); //this needs to be dynamic. 
            doc.Element("Documents");

            XElement document = new XElement("Document");
            XElement name = new XElement("DocumentName");
            XElement numberTotal = new XElement("TotalPageNumber");
            XElement userNumber = new XElement("UserPageNumber");
            XElement docAccuracy = new XElement("DocumentAccuracy");

            document.Add(name);
            document.Add(numberTotal);
            document.Add(userNumber);
            document.Add(docAccuracy);

            doc.Element("documents").Add(document);
        }

        // remove document from user
        // get documentmodel from user
    }
}
