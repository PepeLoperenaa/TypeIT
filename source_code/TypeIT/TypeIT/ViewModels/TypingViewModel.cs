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

        public string inputString;
        public string InputString
        {
            get
            {
                return inputString;
            }
            set
            {
                if (TypingModel.CurrentLetterIndexFromWord == 0 && TypingModel.CurrentWordIndex == 0)
                {
                    TypingModel.StartTime = DateTime.Now;
                    TypingModel.TypingTimer.Start();
                }

                inputString = value;
                TypeWord(TypingModel.CurrentWord);
            }
        }

        public TypingViewModel(NavigationStore navigationStore)
        {
            TypingModel = new TypingModel("../../../Documents/Overlord 1");

            string FileText = TypingModel.Text;

            TypingModel.CurrentWord = GetWord(FileText, TypingModel.CurrentWordIndex);
            TypingModel.TypingTimer.Elapsed += OnTimedEvent;
            TypingModel.TypingTimer.Interval = 1000;

            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
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

        private bool IsWordCorrect(string word)
        {
            if (InputString.Length > 0)
            {
                // Check if you're at the last word in the text
                if (TypingModel.CurrentWordIndex != GetNumberOfWords(TypingModel.Text))
                {
                    // goes to the next word in the text, needs to check for the space to know when to go to next word

                    // check if the end of the word is a space to determine when to break to the next word
                    if (InputString.Last() == ' ')
                    {
                        // check if the word minus the space at the end is equal to the expected word
                        if (InputString[0..^1].Equals(word))
                        {
                            InputLengthTracker = 0;
                            InputString = "";
                            return true;
                        }
                    }
                }
                else
                {
                    // returns true if at the last word so that a space isn't needed to return true and go to the next page

                    if (InputString == TypingModel.CurrentWord)
                    {
                        InputLengthTracker = 0;
                        InputString = "";
                        return true;
                    }
                }
            }
            return false;
        }

        private void ParseWord(string word)
        {
            if (IsWordCorrect(word))
            {
                if (TypingModel.CurrentWordIndex == GetNumberOfWords(TypingModel.Text))
                {
                    TypingModel.CurrentWordIndex = 0;
                    if (TypingModel.HasNextPage())
                    {
                        TypingModel.NextPage();
                    }
                    else
                    {
                        // Go back to home page or say hey you finished the book or something
                    }

                    TypingModel.TextLeft = TypingModel.Text;
                    TypingModel.TextCorrect = "";
                    TypingModel.TextWrong = "";
                    TypingModel.CurrentLetterIndexFromText = 0;
                }
                else
                {
                    TypingModel.CurrentWordIndex++;
                }
                TypingModel.CurrentLetterIndexFromWord = 0;
                TypingModel.CurrentWord = GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);
                return;
            }
        }

        public void TypeWord(string word)
        {
            TypingModel.CurrentMistakes = 0;

            TypingModel.CurrentLetterIndexFromWord = InputString.Length;
            TypingModel.InputCount++;

            if (GetMistakeIndex(word) >= 0)
            {
                if (InputString.Trim() != word)
                {

                    TypingModel.CurrentMistakes = (InputString.Length - GetMistakeIndex(word));
                    TypingModel.TotalMistakes++;
                }
            }

            // Decrement CurrentLetterIndexFromText
            // Not complete yet, still has some bugs
            if (InputLengthTracker > InputString.Length && TypingModel.CurrentLetterIndexFromText > 0 && word.Contains(InputString) && InputString != "")
            {
                TypingModel.CurrentLetterIndexFromText -= (InputLengthTracker - InputString.Length);
            }

            InputLengthTracker = InputString.Length;

            // set the number of letters they can input after making a mistake
            if (word.Contains(InputString.Trim()) && InputString != "")
            {
                if (TypingModel.Text[TypingModel.CurrentLetterIndexFromText] == InputString.Last() && TypingModel.CurrentWordIndex <= GetNumberOfWords(TypingModel.Text))
                {
                    TypingModel.CurrentLetterIndexFromText++;
                }

                TypingModel.ErrorSpace = InputString.Length + TypingModel.CalculateErrorSpace(word);
            }


            TypingModel.TextLeft = TypingModel.Text[(TypingModel.CurrentLetterIndexFromText + TypingModel.CurrentMistakes)..];
            TypingModel.TextWrong = TypingModel.Text[(TypingModel.CurrentLetterIndexFromText)..(TypingModel.CurrentLetterIndexFromText + TypingModel.CurrentMistakes)];
            TypingModel.TextCorrect = TypingModel.Text[0..TypingModel.CurrentLetterIndexFromText];

            ParseWord(word);
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
