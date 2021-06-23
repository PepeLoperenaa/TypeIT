using System.Collections.Generic;
using System.ComponentModel;

namespace TypeIT.Models
{
    public class StatisticsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public double HighestWPM { get; set; }
        public double AverageAccuracy { get; set; }
        public int HoursSpent { get; set; }
        public int TypedWordsTotal { get; set; }
        public List<DailyRecordModel> DailyRecords { get; set; }
        private int _averageWpm;

        // used to calculate accuracy for the page
        public int AverageWpm
        {
            get => _averageWpm;
            set
            {
                _averageWpm = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageWpm)));
            }
        }
        public StatisticsModel(double highestWpm, int averageWpm, double averageAccuracy, int hoursSpent, int typedWordsTotal)
        {
            HighestWPM = highestWpm;
            AverageWpm = averageWpm;
            AverageAccuracy = averageAccuracy;
            HoursSpent = hoursSpent;
            TypedWordsTotal = typedWordsTotal;
        }
    }
}
