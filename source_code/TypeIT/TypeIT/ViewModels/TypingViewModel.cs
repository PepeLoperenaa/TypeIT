using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (InputString == "" && TypingModel.CurrentWordIndex == 0)
                {
                    TypingModel.StartTime = DateTime.Now;
                }
                inputString = value;

                TypeWord(TypingModel.CurrentWord);
                TypingModel.AverageAccuracy = CalculateAccuracy(TypingModel.Text.Length);
                TypingModel.AverageTypingSpeed = CalculateTypingSpeed(TypingModel.CurrentWordIndex);
            }
        }

        public TypingViewModel(NavigationStore navigationStore)
        {
            TypingModel = new TypingModel(File.ReadAllText("../../../Documents/20.txt"));
            TypingModel.CurrentWord = GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);

            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
        }

        public int CalculateErrorSpace(string word)
        {
            int minSpace = 5;
            int calculatedSpace = (int)((word.Length * 1.5) / 2);

            return minSpace > calculatedSpace ? minSpace : calculatedSpace;
        }

        public double CalculateTypingSpeed(int wordNumber)
        {
            return Math.Round((wordNumber / (DateTime.Now - TypingModel.StartTime).TotalSeconds) * 60, 2);
        }

        public double CalculateAccuracy(int totalChars)
        {
            return Math.Round(((Double)(totalChars - TypingModel.TotalMistakes) / totalChars) * 100, 2);
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
                catch (IndexOutOfRangeException ex)
                {
                    return word.Length;
                }
            }
            return -1;
        }

        public void TypeWord(string word)
        {
            TypingModel.CurrentMistakes = 0;

            string text = InputString;

            // set the number of letters they can input after making a mistake
            if (word.Contains(text))
            {
                TypingModel.ErrorSpace = InputString.Length + CalculateErrorSpace(word);
            }

            if (text.Length > 0)
            {
                if (text.Last() == ' ')
                {
                    if (text[0..^1].Equals(word))
                    {
                        TypingModel.CurrentWordIndex++;
                        TypingModel.CurrentWord = GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);
                        InputString = "";
                        return;
                    }
                }
            }

            TypingModel.CurrentMistakes = text.Length - GetMistakeIndex(word);
            TypingModel.TotalMistakes += TypingModel.CurrentMistakes;
        }

        public string GetWord(string text, int wordIndex)
        {
            string[] words = text.Split(' ');

            if (wordIndex < GetNumberOfWords(text))
            {
                return words[wordIndex];
            }
            return null;
        }

        public int GetNumberOfWords(string text)
        {
            return text.Split(' ').Length;
        }
    }
}
