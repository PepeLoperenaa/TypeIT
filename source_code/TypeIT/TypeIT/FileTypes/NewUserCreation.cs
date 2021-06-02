using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TypeIT.FileTypes
{
    class NewUserCreation
    {
        public void newUser() {
            XDocument doc;
        

            doc = new XDocument(new XElement("UserProfile",
                                       new XElement("Name", "Pepe Loperena"), //after specifying the tag, the value should be added.
                                       new XElement("Value",
                                           new XElement("Statistics",
                                               new XElement("WPM"),
                                               new XElement("Average"),
                                               new XElement("HoursSpent"),
                                               new XElement("TypedWords"))),
                                       new XElement("Settings",
                                           new XElement("Theme"),
                                           new XElement("GameMode")),
                                       new XElement("Achievements",
                                           new XElement("Achievement",
                                               new XElement("AchievementName")))));

            doc.Save(Directory.GetCurrentDirectory() + "//PepeLoperena.TypeIT"); //this may need to be changed to where the user wants to save and what name

        }
        public void addingDataIntoAnXml()
        {
            string file = "C:/Users/MyPC/source/repos/XMLChanging/XMLChanging/folder/userFile.TypeIT";
            XDocument doc = XDocument.Load(file);

            doc.Root.Element("Name").Value = "Example";
            doc.Root.Element("Value").Element("Statistics").Element("WPM").Value = "20";
            doc.Root.Element("Value").Element("Statistics").Element("Average").Value = "20";
            doc.Root.Element("Value").Element("Statistics").Element("Accuracy").Value = "20";
            doc.Root.Element("Value").Element("Statistics").Element("HoursSpent").Value = "20";
            doc.Root.Element("Value").Element("Statistics").Element("TypedWords").Value = "20";
            doc.Root.Element("Settings").Element("Theme").Value = "LightMode";
            doc.Root.Element("Settings").Element("GameMode").Value = "Hard";
            doc.Root.Element("Achievements").Element("Achievement").Element("AchievementName").Value = "Yes";
            doc.Save(file);
        }
    }
}
