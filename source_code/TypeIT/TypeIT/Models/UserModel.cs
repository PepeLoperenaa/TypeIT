using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public StatisticsModel statistics { get; set; }
        public string Theme { get; set; }
        public string GameMode { get; set; }
        public List<AchievementModel> Achievements { get; set; }
        public List<DocumentModel> Documents { get; set; }

        public UserModel(string name)
        {
            Name = name;
        }
    }
}
