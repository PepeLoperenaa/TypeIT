﻿using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    class TypingViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public TypingModel TypingModel { get; set; }
        public int InputLengthTracker { get; set; }
        public UserStore currentUser { get; set; }

        public string _inputString;
        /// <summary>
        /// Sets the inputString and calls the TypeWord Method whenever the inputString is updated
        /// </summary>
        public string InputString
        {
            get
            {
                return _inputString;
            }
            set
            {
                _inputString = value;

                if (InputString.Length == 1 && TypingModel.CurrentWordIndex == 0)
                {
                    TypingModel.StartTime = DateTime.Now;
                    TypingModel.TypingTimer.Start();
                }

                TypeWord(TypingModel.CurrentWord);
            }
        }

        public TypingViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            currentUser = userStore;

            TypingModel = new TypingModel("../../../Documents/Overlord 1");

            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));

            // loading the user's game mode
            TypingModel.SelectedDifficulty = (currentUser.CurrentUser.GameMode);

            // set the initial text 
            TypingModel.CharactersLeft = TypingModel.Text;

            // set the initial current word
            TypingModel.CurrentWord = TypingModel.GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);

            // set the in itial displayed time
            TypingModel.SetDisplayTime(currentUser.CurrentUser.Statistics.AverageWPM);

            TypingModel.DisplayAverages();

            // configure timer
            TypingModel.TypingTimer.Elapsed += OnTimedEvent;
            TypingModel.TypingTimer.Interval = 1000;
        }

        /// <summary>
        /// Timer to constantly calculate user accuracy and average speed throughout the page
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            TypingModel.DisplayAverages();

            TypingModel.IncrementTime();
        }

        /// <summary>
        /// This method is the main function for typing the words out.
        /// 
        /// It keeps track of where the user currently is in the document, as well as handling the number of mistakes a user has made
        /// on the current page.
        /// </summary>
        /// <param name="word"></param>
        public void TypeWord(string word)
        {
            TypingModel.CurrentMistakes = 0;

            TypingModel.InputCount++;

            // sets the current mistakes depending on the index at which they made the mistakes, as well as increments the 
            // number of total mistakes the user has made
            if (TypingModel.GetMistakeIndex(word, InputString) >= 0)
            {
                if (InputString.Trim() != word)
                {
                    TypingModel.CurrentMistakes = (InputString.Length - TypingModel.GetMistakeIndex(word, InputString));
                    TypingModel.TotalMistakes++;
                }
            }

            // handle the countdown timer if difficulty is extreme
            TypingModel.HandleExtreme();

            // set the index based on the current length subtracted by the tracker
            TypingModel.Index += InputString.Length - InputLengthTracker;

            // set the length tracker after processing the index
            InputLengthTracker = InputString.Length;

            // set the number of letters they can input after making a mistake
            if (word.Contains(InputString.Trim()) && InputString != "")
            {
                TypingModel.ErrorSpace = InputString.Length + TypingModel.CalculateErrorSpace(word);
            }

            UpdateDisplayText(word);
            ParseWord(word);
        }

        /// <summary>
        /// Updates the three text blocks used to display the different colors whilst typing.
        /// </summary>
        /// <param name="word"></param>
        private void UpdateDisplayText(string word)
        {
            // set the gray text (text left to type)
            TypingModel.CharactersLeft = TypingModel.Text[(TypingModel.Index)..];

            // set text wrong to null before checking so that if word has no wrong characters then there won't be wrong characters displayed
            // !IMPORTANT do not delete
            TypingModel.TextWrong = null;

            if (TypingModel.IsActualExpected(InputString, word))
            {
                // set value of text correct to all correct characters
                TypingModel.TextCorrect = TypingModel.Text[0..TypingModel.Index];
            }
            else
            {
                // set value of text wrong to all wrong characters
                TypingModel.TextWrong = TypingModel.Text[(TypingModel.Index - TypingModel.CurrentMistakes)..(TypingModel.Index)];
            }
        }

        /// <summary>
        /// Determines if the user proceeds to the next word / page by checking if the word which the
        /// user has typed is the word expected
        /// </summary>
        /// <param name="word"></param>
        private void ParseWord(string word)
        {
            if (CanGoToNextWord(word))
            {
                if (TypingModel.CurrentWordIndex == TypingModel.GetNumberOfWords())
                {
                    if (TypingModel.HasNextPage())
                    {
                        TypingModel.NextPage();

                        // doing this here since we need user statistics for calculation
                        TypingModel.SetDisplayTime(currentUser.CurrentUser.Statistics.AverageWPM);
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
                TypingModel.CurrentWord = TypingModel.GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);

                InputLengthTracker = 0;
                InputString = "";

                return;
            }
        }

        /// <summary>
        /// Checking if the user can go to the next word depending if the word is written correctly or not
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool CanGoToNextWord(string word)
        {
            if (InputString.Length > 0)
            {
                // check if you're at the last word to see if the space is needed to continue
                // important otherwise the text will proceed without a space needed
                if (TypingModel.CurrentWordIndex == TypingModel.GetNumberOfWords())
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
    }
}