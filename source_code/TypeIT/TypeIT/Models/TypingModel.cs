﻿using System;
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
        public string CurrentWord { get; set; }
        public int ErrorSpace { get; set; }
        public int TotalMistakes { get; set; }
        public double AverageTypingSpeed { get; set; }
        public double AverageAccuracy { get; set; }
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
