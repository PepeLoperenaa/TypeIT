using System.IO;
using System.Xml.Linq;

namespace TypeIT.FileTypes
{
    class NewUserCreation
    {
        public void newUser(string name, string fileName)
        {
            XDocument doc;
        

            doc = new XDocument(new XElement("UserProfile",
                                       new XElement("Name", name), //after specifying the tag, the value should be added.
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

            doc.Save(Directory.GetCurrentDirectory() + "//PepeLoperena.TypeIT");
            //when we save in the current directory, it will save the data in the debug folder.
        }

        public void getElementsFromTags(string filePath, string tag)
        {
            XDocument doc = XDocument.Load(filePath);
            //XDocument parse = XDocument.Parse(fileChris);

            foreach (XElement lev2 in doc.Descendants(tag))
            {
                string lev2Value = (string)lev2;
                //return a list of string. 
            }
        }
        public void addingDataIntoAnXml(string filePath) //to add information into the element
        {
            //string file = "C:/Users/MyPC/source/repos/XMLChanging/XMLChanging/folder/userFile.TypeIT";
            XDocument doc = XDocument.Load(filePath);

            doc.Root.Element("Name").Value = "Example";
            doc.Root.Element("Value").Element("Statistics").Element("WPM").Value = "20";
            doc.Root.Element("Value").Element("Statistics").Element("Average").Value = "20";
            doc.Root.Element("Value").Element("Statistics").Element("Accuracy").Value = "20";
            doc.Root.Element("Value").Element("Statistics").Element("HoursSpent").Value = "20";
            doc.Root.Element("Value").Element("Statistics").Element("TypedWords").Value = "20";
            doc.Root.Element("Settings").Element("Theme").Value = "LightMode";
            doc.Root.Element("Settings").Element("GameMode").Value = "Hard";
            doc.Root.Element("Achievements").Element("Achievement").Element("AchievementName").Value = "Yes";
            doc.Save(filePath);
        }
    }
}
