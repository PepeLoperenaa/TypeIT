using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using TypeIT.Objects;

namespace TypeIT.Models
{
    class TypingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public BookModel Document { get; set; }
        // used to keep track of typing individual words
        public int CurrentMistakes { get; set; }
        // keep track of which part of the text is being typed
        public int CurrentWordIndex { get; set; }
        // used to calculate accuracy for the page
        public int TotalMistakes { get; set; }
        // used to keep track of where the user is in the text and replace colors
        public int Index { get; set; }
        public int InputCount { get; set; }
        public double HighestSpeed { get; set; }
        public DateTime StartTime { get; set; }
        public string Text { get; set; }

        //Binded values
        private string _textCorrect;
        private string _textWrong;
        private string _textLeft;
        private string _currentWord;
        private double _averageTypingSpeed;
        private double _averageAccuracy;
        private Timer _typingTimer;
        private int _errorSpace;
        private int _pageNumber;

        public int PageNumber 
        { 
            get => _pageNumber; 
            set
            {
                _pageNumber = value;
                Text = GetTextFromPage(PageNumber);
            }
        }

        public string TextCorrect
        {
            get => _textCorrect;
            set
            {
                _textCorrect = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextCorrect)));
            }
        }

        public string TextWrong
        {
            get => _textWrong;
            set
            {
                _textWrong = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextWrong)));
            }
        }

        public string CharactersLeft
        {
            get => _textLeft;
            set
            {
                _textLeft = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CharactersLeft)));
            }
        }

        public int ErrorSpace
        {
            get => _errorSpace;
            set
            {
                _errorSpace = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorSpace)));
            }
        }

        public Timer TypingTimer
        {
            get => _typingTimer;
            set
            {
                _typingTimer = value;
            }
        }

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

        public TypingModel(string document)
        {
            Document = new Objects.BookModel(document);
            PageNumber = 0;
            CurrentMistakes = 0;
            HighestSpeed = 0;
            TotalMistakes = 0;
            AverageAccuracy = 0;
            AverageTypingSpeed = 0;
            CurrentWordIndex = 0;
            ErrorSpace = 5;
            Text = GetTextFromPage(PageNumber);
            TypingTimer = new Timer();
        }

        private void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }
        public int CalculateErrorSpace(string word)
        {
            int minSpace = 5;
            int calculatedSpace = (int)((word.Length * 1.5) / 2);

            return minSpace > calculatedSpace ? minSpace : calculatedSpace;
        }

        public double CalculateTypingSpeed(int wordNumber)
        {
            double speed = Math.Round((wordNumber / (DateTime.Now - StartTime).TotalSeconds) * 60, 2);
            if (speed > HighestSpeed)
            {
                HighestSpeed = speed;
            }
            return speed;
        }

        public double CalculateAccuracy(int totalChars)
        {
            return Math.Round(((Double)(totalChars - TotalMistakes) / totalChars) * 100, 2);
        }

        public string GetTextFromPage(int pageNumber)
        {
            return Document.GetPageByPageNumber(pageNumber).Text;
        }

        public bool HasNextPage()
        {
            if (PageNumber < Document.GetNumberOfPages())
            {
                return true;
            }
            return false;
        }

        public void NextPage()
        {
            PageNumber++;
        }
        
        public void PreviousPage()
        {
            PageNumber--;
        }
    }
}
