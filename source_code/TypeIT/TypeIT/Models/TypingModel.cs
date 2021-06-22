using System;
using System.ComponentModel;
using System.Timers;
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
        public DateTime StartTime { get; set; }
        public Difficulty SelectedDifficulty { get; set; }
        public bool Alive { get; set; }

        //Binded values
        private DocumentModel _document;
        private string _averageAccuracy;
        private string _textCorrect;
        private string _textWrong;
        private string _textLeft;
        private string _currentWord;
        private string _timeDisplay;
        private string _averageTypingSpeed;
        private int _errorSpace;
        private int _pageNumber;
        private int _timeCounter;
        private int _totalMistakes;

        // used to calculate accuracy for the page
        public int TotalMistakes
        {
            get => _totalMistakes;
            set { _totalMistakes = value; }
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
                // Have to do - 1 here since programmers use base 1 for arrays, but normal people
                // use base 1 (Makes sure we display page 1 and get from array[0])
                Text = GetTextFromPage(PageNumber);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageNumber)));
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

        public Timer TypingTimer { get; private set; }

        public string CurrentWord
        {
            get => _currentWord;
            set
            {
                _currentWord = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentWord)));
            }
        }

        public string AverageTypingSpeed
        {
            get => _averageTypingSpeed;
            set
            {
                _averageTypingSpeed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageTypingSpeed)));
            }
        }

        public string AverageAccuracy
        {
            get => _averageAccuracy;
            set
            {
                _averageAccuracy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageAccuracy)));
            }
        }

        public DocumentModel Document
        {
            get => _document;
            set
            {
                _document = value;

                PageNumber = Document.UserPageNumber;
            }
        }

        public TypingModel()
        {
            CurrentMistakes = 0;
            HighestSpeed = 0;
            TotalMistakes = 0;
            Alive = true;
            CurrentWordIndex = 0;
            ErrorSpace = 5;
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

        /// <summary>
        /// Calculates the number of letters which a user can input based on the length of the current word
        /// </summary>
        /// <param name="word"></param>
        /// <returns>The space a user has for error on the current word</returns>
        public int CalculateErrorSpace(string word)
        {
            int minSpace = 5;
            int calculatedSpace = (int)((word.Length * 1.5) / 2);

            return minSpace > calculatedSpace ? minSpace : calculatedSpace;
        }

        /// <summary>
        /// Calculates the user's typing speed 
        /// </summary>
        /// <param name="wordNumber"></param>
        /// <returns>The average typing speed of a user</returns>
        public double CalculateTypingSpeed(int wordNumber)
        {
            double speed = Math.Round((wordNumber / (DateTime.Now - StartTime).TotalSeconds) * 60, 2);
            if (speed > HighestSpeed)
            {
                HighestSpeed = speed;
            }

            return speed;
        }

        /// <summary>
        /// Calculates the user's accuracy
        /// </summary>
        /// <param name="totalChars"></param>
        /// <returns>The average accuracy of a user</returns>
        public double CalculateAccuracy(int totalChars)
        {
            return Math.Round(((Double)(totalChars - TotalMistakes) / totalChars) * 100, 2);
        }

        /// <summary>
        /// Retrieves the text from the current word being typed
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns>The text from the page</returns>
        public string GetTextFromPage(int pageNumber)
        {
            if (Document.GetPageByPageNumber(PageNumber - 1) == null)
            {
                _pageNumber = 1;
            }

            return Document.GetPageByPageNumber(PageNumber - 1).Text;
        }

        /// <summary>
        /// Calculates the amount of time in seconds which a user has based on the users
        /// average typing speed.
        /// </summary>
        /// <param name="wordCount"></param>
        /// <param name="averageWpm"></param>
        /// <returns>The time limit which the user has to complete the current page</returns>
        public static int CalculateMaxTimeInSeconds(double wordCount, double averageWpm)
        {
            if (averageWpm > 6)
            {
                return (int)((wordCount * 60) / (averageWpm)) + 5;
            }
            else
            {
                return (int)((wordCount * 60) / 30) + 5;
            }
        }

        /// <summary>
        /// Determines if the document has another page after the current one
        /// </summary>
        /// <returns>Whether the next page exists or not</returns>
        public bool HasNextPage()
        {
            if (PageNumber < Document.GetNumberOfPages())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clears the data on the previous page and sets the text to that of the current one
        /// </summary>
        public void NavigatePage()
        {
            // indexes
            CurrentWordIndex = 0;
            Index = 0;

            // text blocks
            CharactersLeft = Text.Trim();
            TextCorrect = "";
            TextWrong = "";

            // mistake trackers
            CurrentMistakes = 0;
            TotalMistakes = 0;

            // stat trackers
            DisplayAverages();
            HighestSpeed = 0;

            // time trackers
            TypingTimer.Stop();
        }

        public void DisplayAverages()
        {
            if (SelectedDifficulty == Difficulty.Easy)
            {
                AverageAccuracy = "N/A";
                AverageTypingSpeed = "N/A";
            }
            else
            {
                AverageAccuracy = CalculateAccuracy(InputCount).ToString();

                AverageTypingSpeed = ((int)CalculateTypingSpeed(CurrentWordIndex)).ToString();

                if (Double.Parse(AverageAccuracy) < 0)
                {
                    AverageAccuracy = "0";
                }
            }
        }

        /// <summary>
        /// Resets the current page values, and sets the text to that of the next page
        /// </summary>
        public void NextPage()
        {
            PageNumber++;

            NavigatePage();
        }

        /// <summary>
        /// Resets the current page values, and sets the text to that of the previous page
        /// </summary>
        public void PreviousPage()
        {
            PageNumber--;

            NavigatePage();
        }

        /// <summary>
        /// Gets the number of words on the current page
        /// </summary>
        /// <returns>Word count for current page</returns>
        public int GetNumberOfWords()
        {
            return Text.Split(' ').Length - 1;
        }

        /// <summary>
        /// Increments or decrements the time counter every game tick, depending on the selected difficulty
        /// </summary>
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

        /// <summary>
        /// Handles the taxing on the time limit if a user makes a mistake in Extreme difficulty
        /// </summary>
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

        /// <summary>
        /// Handles what happens per game tick based on the difficulty setting
        /// </summary>
        private void OnCounterChange()
        {
            // set the time span string whenever the counter changes (always increments or decrements)
            TimeDisplay = TimeSpan.FromSeconds(TimeCounter).ToString("mm':'ss");
        }

        /// <summary>
        /// Checks if what the user has typed is correct and as exptected
        /// </summary>
        /// <param name="actual">What the user has typed</param>
        /// <param name="expected">What the user was expected to type</param>
        /// <returns>Whether the user has typed the correct input so far</returns>
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

        /// <summary>
        /// Updates the display timer at the start of the page
        /// </summary>
        /// <param name="averageWpm"></param>
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

        /// <summary>
        /// Gets the word at a specific index of the page
        /// </summary>
        /// <param name="text">The text to get the word from</param>
        /// <param name="wordIndex">The index to get the word from</param>
        /// <returns>The word at the specified index</returns>
        public string GetWord(string text, int wordIndex)
        {
            string[] words = text.Split(' ');

            if (wordIndex <= GetNumberOfWords())
            {
                return words[wordIndex];
            }

            return null;
        }

        /// <summary>
        /// Gets the index of the current user input at which a mistake was made
        /// </summary>
        /// <param name="word">The word the user was extected to type</param>
        /// <param name="input">The string which the user inputted</param>
        /// <returns>The index at which a mistake was made | returns - 1 if none</returns>
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