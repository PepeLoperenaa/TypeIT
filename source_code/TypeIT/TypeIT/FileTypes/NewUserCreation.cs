using System.IO;
using System.Xml.Linq;

namespace TypeIT.FileTypes
{
    class NewUserCreation
    {
        public void newUser(string name)
        {
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

            doc.Save(Directory.GetCurrentDirectory() + "//PepeLoperena.TypeIT");
            //when we save in the current directory, it will save the data in the debug folder.
        }

        public void getElementsFromTags(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            //XDocument parse = XDocument.Parse(fileChris);

            foreach (XElement lev2 in doc.Descendants("level2"))
            {
                string lev2Value = (string)lev2;
            }
        }

    }
}
