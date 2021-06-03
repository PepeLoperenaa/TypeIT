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
        public UserStore currentUser { get; set; }
        public ObservableCollection<AchievementModel> Achievements { get; set; }

        public AchievementsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            Achievements = new ObservableCollection<AchievementModel>();
            currentUser = userStore;
            loadAchievements();
        }

        private void loadAchievements()
        {
            //Loading the document that stores the achievements
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("../../../FileTypes/TypeitFiles/achievements.TypeIT");

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
