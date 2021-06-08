using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using TypeIT.Objects;
using TypeIT.Utilities;

namespace TypeIT.Models
{
    class TypingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool Taxed { get; set; }
        // used to keep track of typing individual words
        public int CurrentMistakes { get; set; }
        // keep track of which part of the text is being typed
        public int CurrentWordIndex { get; set; }
        // used to keep track of where the user is in the text and replace colors
        public int Index { get; set; }
        public int InputCount { get; set; }
        public double HighestSpeed { get; set; }
        public string Text { get; set; }
        public DocumentModel Document { get; set; }
        public DateTime StartTime { get; set; }
        public Difficulty SelectedDifficulty { get; set; }

        //Binded values
        private Timer _typingTimer;
        private double _averageAccuracy;
        private string _textCorrect;
        private string _textWrong;
        private string _textLeft;
        private string _currentWord;
        private string _timeDisplay;
        private int _averageTypingSpeed;
        private int _errorSpace;
        private int _pageNumber;
        private int _timeCounter;
        private int _totalMistakes;

        // used to calculate accuracy for the page
        public int TotalMistakes
        {
            get => _totalMistakes;
            set
            {
                _totalMistakes = value;
            }
        }

        public string TimeDisplay
        {
            get => _timeDisplay;
            set
            {
                _timeDisplay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeDisplay)));
            }
        }

        public int TimeCounter
        {
            get => _timeCounter;
            set
            {
                _timeCounter = value;

                OnCounterChange();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeCounter)));
            }
        }

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

        public int AverageTypingSpeed
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

        public TypingModel()
        {
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

        public static int CalculateMaxTimeInSeconds(double wordCount, double averageWpm)
        {
            if (averageWpm > 6)
            {
                return (int)((wordCount * 60) / (averageWpm + 10)) + 5;
            }
            else
            {
                return (int)((wordCount * 60) / 30) + 5;
            }
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

            // indexes
            CurrentWordIndex = 0;
            Index = 0;

            PageNumber++;

            // text blocks
            CharactersLeft = Text;
            TextCorrect = "";
            TextWrong = "";

            // mistake trackers
            CurrentMistakes = 0;
            TotalMistakes = 0;

            // stat trackers
            AverageAccuracy = 0;
            AverageTypingSpeed = 0;
            HighestSpeed = 0;

            // time trackers
            TypingTimer.Stop();
        }

        public void PreviousPage()
        {
            PageNumber--;
        }

        public int GetNumberOfWords()
        {
            return Text.Split(' ').Length - 1;
        }

        public void IncrementTime()
        {
            if (SelectedDifficulty == Difficulty.Easy || SelectedDifficulty == Difficulty.Medium)
            {
                TimeCounter++;
            }
            else
            {
                TimeCounter--;
            }
        }

        public void HandleExtreme()
        {
            if (SelectedDifficulty == Difficulty.Extreme)
            {
                if (CurrentMistakes == 0)
                {
                    Taxed = false;
                }

                // reduce time left by 1 second if difficulty is extreme
                if (CurrentMistakes == 1 && Taxed == false)
                {
                    TimeCounter -= 1;
                    Taxed = true;
                }
            }
        }

        private void OnCounterChange()
        {
            if (SelectedDifficulty == Difficulty.Hard || SelectedDifficulty == Difficulty.Extreme)
            {
                // fail the user if time runs out
                if (TimeCounter <= 0)
                {
                    // go back to main menu or something
                }
            }

            // set the time span string whenever the counter changes (always increments or decrements)
            TimeDisplay = TimeSpan.FromSeconds(TimeCounter).ToString("mm':'ss");
        }

        public bool IsActualExpected(string actual, string expected)
        {
            if (actual.Length >= 0 && actual.Length <= expected.Length)
            {
                if (expected.Substring(0, actual.Length) == actual)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetDisplayTime(int averageWpm)
        {
            if (SelectedDifficulty == Difficulty.Easy || SelectedDifficulty == Difficulty.Medium)
            {
                TimeCounter = 0;
            }
            else
            {
                TimeCounter = TypingModel.CalculateMaxTimeInSeconds(GetNumberOfWords(), averageWpm);
            }
        }
        public string GetWord(string text, int wordIndex)
        {
            string[] words = text.Split(' ');

            if (wordIndex <= GetNumberOfWords())
            {
                return words[wordIndex];
            }
            return null;
        }

        public int GetMistakeIndex(string word, string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                try
                {
                    if (word[i] != input[i])
                    {
                        return i;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    return word.Length;
                }
            }
            return -1;
        }
    }
}
