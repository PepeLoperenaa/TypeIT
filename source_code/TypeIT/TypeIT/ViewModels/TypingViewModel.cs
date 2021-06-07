using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class TypingViewModel : ViewModelBase
    {

        public ICommand NavigateHomeCommand { get; }
        public TypingModel TypingModel { get; set; }
        public string LatestCorrect { get; set; }
        public int InputLengthTracker { get; set; }

        public UserStore currentUser { get; set; }
        public string inputString;
        public string InputString
        {
            get
            {
                return inputString;
            }
            set
            {
                if (InputString != null)
                {
                    if (InputString.Length > 0 && TypingModel.CurrentWordIndex == 0)
                    {
                        TypingModel.StartTime = DateTime.Now;
                        TypingModel.TypingTimer.Start();
                    }
                }

                inputString = value;
                TypeWord(TypingModel.CurrentWord);
            }
        }

        public TypingViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            TypingModel = new TypingModel("../../../Documents/Overlord 1");

            TypingModel.CharactersLeft = TypingModel.Text;

            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            //Current user
            currentUser = userStore;

            TypingModel.CurrentWord = GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);
            TypingModel.TypingTimer.Elapsed += OnTimedEvent;
            TypingModel.TypingTimer.Interval = 1000;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            TypingModel.AverageAccuracy = TypingModel.CalculateAccuracy(TypingModel.InputCount);
            if (TypingModel.AverageAccuracy < 0)
            {
                TypingModel.AverageAccuracy = 0;
            }
            TypingModel.AverageTypingSpeed = TypingModel.CalculateTypingSpeed(TypingModel.CurrentWordIndex);
        }

        public void TypeWord(string word)
        {
            TypingModel.CurrentMistakes = 0;

            TypingModel.InputCount++;

            if (GetMistakeIndex(word) >= 0)
            {
                if (InputString.Trim() != word)
                {
                    TypingModel.CurrentMistakes = (InputString.Length - GetMistakeIndex(word));
                    TypingModel.TotalMistakes++;
                }
            }

            // set the index based on the current length subtracted by the tracker
            TypingModel.Index += InputString.Length - InputLengthTracker;

            // set the length tracker after processing the index
            InputLengthTracker = InputString.Length;

            // set the number of letters they can input after making a mistake
            if (word.Contains(InputString.Trim()) && InputString != "")
            {
                TypingModel.ErrorSpace = InputString.Length + TypingModel.CalculateErrorSpace(word);
            }

            TypingModel.CharactersLeft = TypingModel.Text[(TypingModel.Index)..];

            // set text wrong to null before checking so that if word has no wrong characters then there won't be wrong characters displayed
            // !IMPORTANT do not delete
            TypingModel.TextWrong = null;

            if (IsCurrentInputCorrect(word))
            {
                // set value of text correct to all correct characters
                TypingModel.TextCorrect = TypingModel.Text[0..TypingModel.Index];
            }
            else
            {
                // set value of text wrong to all wrong characters
                TypingModel.TextWrong = TypingModel.Text[(TypingModel.Index - TypingModel.CurrentMistakes)..(TypingModel.Index)];
            }

            ParseWord(word);
        }

        private void ParseWord(string word)
        {
            if (CanGoToNextWord(word))
            {
                if (TypingModel.CurrentWordIndex == GetNumberOfWords(TypingModel.Text))
                {
                    if (TypingModel.HasNextPage())
                    {
                        TypingModel.NextPage();
                    }
                    else
                    {
                        // Go back to home page or say hey you finished the book or something
                    }
                }
                else
                {
                    TypingModel.CurrentWordIndex++;
                }
                TypingModel.CurrentWord = GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);

                InputLengthTracker = 0;
                InputString = "";

                return;
            }
        }

        private bool CanGoToNextWord(string word)
        {
            if (InputString.Length > 0)
            {
                // check if you're at the last word to see if the space is needed to continue
                // important otherwise the text will proceed without a space needed
                if (TypingModel.CurrentWordIndex == GetNumberOfWords(TypingModel.Text))
                {
                    // check if the input is equal to the word (no need to trim)
                    if (InputString == TypingModel.CurrentWord)
                    {
                        return true;
                    }
                }
                else
                {
                    // goes to the next word in the text, needs to check for the space to know when to go to next word

                    // check if the end of the word is a space to determine when to break to the next word
                    if (InputString.Last() == ' ')
                    {
                        // check if the word minus the space at the end is equal to the expected word
                        if (InputString[0..^1].Equals(word))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsCurrentInputCorrect(string word)
        {
            if (InputString.Length >= 0 && InputString.Length <= word.Length)
            {
                if (word.Substring(0, InputString.Length) == InputString)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetMistakeIndex(string word)
        {
            for (int i = 0; i < InputString.Length; i++)
            {
                try
                {
                    if (word[i] != InputString[i])
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

        public string GetWord(string text, int wordIndex)
        {
            string[] words = text.Split(' ');

            if (wordIndex <= GetNumberOfWords(text))
            {
                return words[wordIndex];
            }
            return null;
        }

        public int GetNumberOfWords(string text)
        {
            int number = text.Split(' ').Length - 1;
            return number;
        }
    }
}
