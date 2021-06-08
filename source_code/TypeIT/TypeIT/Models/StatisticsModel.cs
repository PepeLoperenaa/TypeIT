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

        public StatisticsModel(string UserName)
        {
            string parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + UserName + ".TypeIT", "HighestWPM").First();
            HighestWPM = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + UserName + ".TypeIT", "AverageWPM").First();
            AverageWPM = (parameter == "") ? 0 : Int32.Parse(parameter);

            parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + UserName + ".TypeIT", "AverageAccuracy").First();
            AverageAccuracy = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + UserName + ".TypeIT", "HoursSpent").First();
            HoursSpent = (parameter == "") ? 0 : Convert.ToDouble(parameter);

            parameter = XmlHandler.getElementsFromTags("../../../FileTypes/Users/" + UserName + ".TypeIT", "TypedTypedWordsTotalWords").First();
            TypedWordsTotal = (parameter == "") ? 0 : Convert.ToInt32(parameter);
        }
    }
}
