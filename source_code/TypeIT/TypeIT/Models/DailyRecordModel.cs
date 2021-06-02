using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Models
{
    public class DailyRecordModel
    {
        public DateTime Date { get; set; }
        public double WPM { get; set; }
        public double Average { get; set; }
        public double Accuracy { get; set; }
    }
}
