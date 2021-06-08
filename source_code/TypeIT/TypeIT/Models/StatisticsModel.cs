using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeIT.FileTypes;

namespace TypeIT.Models
{
    public class StatisticsModel
    {
        public double HighestWPM { get; set; }
        public int AverageWPM { get; set; }
        public double AverageAccuracy { get; set; }
        public double HoursSpent { get; set; }
        public int TypedWordsTotal { get; set; }
        public List<DailyRecordModel> DailyRecords { get; set; }

        public StatisticsModel(double highestWPM, int averageWPM, double averageAccuracy, double hoursSpent, int typedWordsTotal)
        {
            HighestWPM = highestWPM;
            AverageWPM = averageWPM;
            AverageAccuracy = averageAccuracy;
            HoursSpent = hoursSpent;
            TypedWordsTotal = typedWordsTotal;
        }
    }
}
