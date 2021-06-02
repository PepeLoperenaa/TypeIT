using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Models
{
    public class StatisticsModel
    {
        public double HighestWPM { get; set; }
        public double AverageWPM { get; set; }
        public double AverageAccuracy { get; set; }
        public double HoursSpent { get; set; }
        public int TypedWordsTotal { get; set; }
        public List<DailyRecordModel> DailyRecords {get; set;}
    }
}
