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
        public string inputString;
        public string InputString
        {
            get
            {
                return inputString;
            }
            set
            {
                if (TypingModel.CurrentLetterIndex == 0 && TypingModel.CurrentWordIndex == 0)
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
            TypingModel.AverageAccuracy = TypingModel.CalculateAccuracy(TypingModel.CurrentLetterFromText);
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
                // check if the end of the word is a space to determine when to break to the next word
                if (InputString.Last() == ' ')
                {
                    // check if the word minus the space at the end is equal to the expected word
                    if (InputString[0..^1].Equals(word))
                    {
                        InputString = "";
                        return true;
                    }
                }
            }
            return false;
        }

        public void TypeWord(string word)
        {
            TypingModel.CurrentMistakes = 0;

            // set the number of letters they can input after making a mistake
            if (word.Contains(InputString))
            {
                TypingModel.ErrorSpace = InputString.Length + TypingModel.CalculateErrorSpace(word);
            }

            if (IsWordCorrect(word))
            {
                if (TypingModel.CurrentWordIndex == GetNumberOfWords(TypingModel.Text))
                {
                    TypingModel.CurrentWordIndex = 0;
                    TypingModel.CurrentLetterIndex = 0;
                    if (TypingModel.HasNextPage())
                    {
                        TypingModel.NextPage();
                        TypingModel.CurrentWord = GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);
                    }
                    else
                    {
                        // Go back to home page
                    }
                }
                else
                {
                    TypingModel.CurrentWordIndex++;
                    TypingModel.CurrentLetterIndex = 0;
                    TypingModel.CurrentWord = GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);
                }
                return;
            }

            TypingModel.CurrentLetterIndex = InputString.Length;
            TypingModel.CurrentLetterFromText++;

            if (GetMistakeIndex(word) >= 0)
            {
                TypingModel.CurrentMistakes = InputString.Length - GetMistakeIndex(word);
                TypingModel.TotalMistakes++;
            }
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
