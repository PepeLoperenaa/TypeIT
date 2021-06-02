using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Models
{
    class AchievementModel
    {
        public string Title { get; set; }
        public string Desc { get; set; }

        public AchievementModel(string title, string desc)
        {
            Title = title;
            Desc = desc;
        }
    }
}
