using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class AchievementsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ObservableCollection<AchievementModel> Achievements { get; set; }

        public AchievementsViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
            Achievements = new ObservableCollection<AchievementModel>();

            loadAchievements();
        }

        private void loadAchievements()
        {
            //Loading the document that stores the achievements
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("../../../FileTypes/achievements.TypeIT");

            //Getting the achievements
            XmlNodeList titles = xmlDocument.GetElementsByTagName("AchievementName");
            XmlNodeList description = xmlDocument.GetElementsByTagName("Description");

            //Adding the achievements to the field that is binded to the view
            for (int i = 0; i < titles.Count; i++)
            {
                AchievementModel achievementModel = new AchievementModel(titles[i].InnerText, description[i].InnerText);
                Achievements.Add(achievementModel);
            }
        }
    }
}
