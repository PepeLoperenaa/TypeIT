using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Models
{
    class TypingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Text { get; set; }
        public int CurrentMistakes { get; set; }
        public int CurrentWordIndex { get; set; }
        public int ErrorSpace { get; set; }
        public int TotalMistakes { get; set; }

        //Binded values
        private string _currentWord;
        private double _averageTypingSpeed;
        private double _averageAccuracy;

        public string CurrentWord
        {
            get => _currentWord;
            set
            {
                _currentWord = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentWord)));
            }
        }

        public double AverageTypingSpeed
        {
            get => _averageTypingSpeed;
            set
            {
                _averageTypingSpeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageTypingSpeed)));
            }
        }

        public double AverageAccuracy
        {
            get => _averageAccuracy;
            set
            {
                _averageAccuracy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageAccuracy)));
            }
        }

        public DateTime StartTime { get; set; }

        public TypingModel(string text)
        {
            CurrentMistakes = 0;
            TotalMistakes = 0;
            AverageAccuracy = 0;
            AverageTypingSpeed = 0;
            CurrentWordIndex = 0;
            Text = text;


        }

        private void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }
    }
}
