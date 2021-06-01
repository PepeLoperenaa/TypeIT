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

    }
}
