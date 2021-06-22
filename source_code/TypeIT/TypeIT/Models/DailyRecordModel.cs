using System;

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
