using System;
using System.Globalization;
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
    internal class TypingViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public TypingModel TypingModel { get; private set; }
        private int InputLengthTracker { get; set; }
        public UserStore CurrentUser { get; private set; }

        private string _inputString;

        /// <summary>
        /// Sets the inputString and calls the TypeWord Method whenever the inputString is updated
        /// </summary>
        public string InputString
        {
            get => _inputString;
            set
            {
                _inputString = value;

                // Set the timer whenever you start typing on a new page
                if (InputString.Length == 1 && TypingModel.CurrentWordIndex == 0 && TypingModel.Alive)
                {
                    TypingModel.StartTime = DateTime.Now;
                    TypingModel.TypingTimer.Start();
                }

                TypeWord(TypingModel.CurrentWord);
                
                // Check if the user has failed (based on timer) whenever they input
                // This is important since Extreme difficulty reduces your time left whenever
                //  you make a mistake.
                CheckIfFailed();
            }
        }

        public TypingViewModel(NavigationStore navigationStore, UserStore UserStore, DocumentModel document)
        {
            CurrentUser = UserStore;

            TypingModel = new TypingModel {Document = document};

            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore,
                () => new DashboardViewModel(navigationStore, UserStore));

            // loading the user's game mode
            TypingModel.SelectedDifficulty = (CurrentUser.CurrentUser.GameMode);

            // set the initial text 
            TypingModel.CharactersLeft = TypingModel.Text;

            // set the initial current word
            TypingModel.CurrentWord = TypingModel.GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);

            // set the in itial displayed time
            TypingModel.SetDisplayTime(CurrentUser.CurrentUser.Statistics.AverageWpm);

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
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (TypingModel.Alive)
            {
                TypingModel.DisplayAverages();

                TypingModel.IncrementTime();
            }

            // Need to check with the dispatcher thread since the CheckIfFailed() method needs it
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                CheckIfFailed();
            });
            
        }

        /// <summary>
        /// This method is the main function for typing the words out.
        /// 
        /// It keeps track of where the user currently is in the document, as well as handling the number of mistakes a user has made
        /// on the current page.
        /// </summary>
        /// <param name="word"></param>
        private void TypeWord(string word)
        {
            if (TypingModel.Alive)
            {
                TypingModel.CurrentMistakes = 0;

                TypingModel.InputCount++;

                // sets the current mistakes depending on the index at which they made the mistakes, as well as increments the 
                // number of total mistakes the user has made
                if (TypingModel.GetMistakeIndex(word, InputString) >= 0)
                {
                    if (InputString.Trim() != word)
                    {
                        TypingModel.CurrentMistakes =
                            (InputString.Length - TypingModel.GetMistakeIndex(word, InputString));
                        TypingModel.TotalMistakes++;
                    }
                }

                // handle the countdown timer if difficulty is extreme
                TypingModel.HandleExtreme();

                // set the index based on the current length subtracted by the tracker
                TypingModel.Index += InputString.Length - InputLengthTracker;

                // set the length tracker after processing the index
                InputLengthTracker = InputString.Length;


                if (TypingModel.Index > TypingModel.Text.Length)
                {
                    return;
                }

                // set the number of letters they can input after making a mistake
                if (word.Contains(InputString.Trim()) && InputString != "")
                {
                    TypingModel.ErrorSpace = InputString.Length + TypingModel.CalculateErrorSpace(word);
                }

                UpdateDisplayText(word);
                ParseWord(word);
            }
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

            // shorten the text displayed when the user has typed 175 words, and if the total number of words is more than 280
            if (TypingModel.GetNumberOfWords() > 280)
            {
                if (TypingModel.CurrentWordIndex % 175 == 0 && TypingModel.CurrentWordIndex != 0)
                {
                    TypingModel.Text = TypingModel.Text[TypingModel.Index..];
                    TypingModel.CurrentWordIndex = 0;
                    TypingModel.Index = 0;
                }
            }

            if (TypingModel.IsActualExpected(InputString, word))
            {
                // set value of text correct to all correct characters
                TypingModel.TextCorrect = TypingModel.Text[0..TypingModel.Index];
            }
            else
            {
                // set value of text wrong to all wrong characters
                TypingModel.TextWrong =
                    TypingModel.Text[(TypingModel.Index - TypingModel.CurrentMistakes)..(TypingModel.Index)];
            }
        }

        /// <summary>
        /// Determines if the user proceeds to the next word / page by checking if the word which the
        /// user has typed is the word expected
        /// </summary>
        /// <param name="word"></param>
        private void ParseWord(string word)
        {
            // Check if the user is allowed to proceed to the next word
            if (!CanGoToNextWord(word))
            {
                return;
            }

            if (TypingModel.CurrentWordIndex == TypingModel.GetNumberOfWords())
            {
                UpdateUserXml();

                string filePath = $"../../../FileTypes/Users/{CurrentUser.CurrentUser.Name}.TypeIT";

                // Sets the user's average wpm
                CurrentUser.CurrentUser.Statistics.AverageWpm =
                    int.Parse(XmlHandler.GetElementsFromTags(filePath, "AverageWPM").FirstOrDefault() ?? "Error");

                // Sets the user's average accuracy
                CurrentUser.CurrentUser.Statistics.AverageAccuracy =
                    int.Parse(XmlHandler.GetElementsFromTags(filePath, "AverageAccuracy").FirstOrDefault() ?? "Error");

                if (TypingModel.HasNextPage())
                {
                    
                    TypingModel.NextPage();

                    // doing this here since we need user statistics for calculation
                    TypingModel.SetDisplayTime(CurrentUser.CurrentUser.Statistics.AverageWpm);
                }
                else
                {
                    TypingModel.Alive = false;

                    TypingModel.TypingTimer.Stop();
                    

                    AchievementHandler.FinishBookAchievements(CurrentUser, TypingModel.Document.GetNumberOfPages(), TypingModel.Document.Name);

                    //Custom messagebox
                    var res = Xceed.Wpf.Toolkit.MessageBox.Show(
                        "You have finished this book!",
                        "Back to Home",
                        MessageBoxButton.OK
                    );

                    if ("OK" == res.ToString())
                    {
                        NavigateHomeCommand.Execute(null);
                    }
                }

                CurrentUser.CurrentUser.RefreshUserModel();
            }
            else
            {
                TypingModel.CurrentWordIndex++;
            }

            // Update the current owrd
            TypingModel.CurrentWord = TypingModel.GetWord(TypingModel.Text, TypingModel.CurrentWordIndex);

            // Reset input string 
            InputLengthTracker = 0;
            InputString = "";
        }

        /// <summary>
        /// Updates the user's file whenever a page is finished
        /// </summary>
        private void UpdateUserXml()
        {

            // determines the average | if difficulty is easy, sets the averages to be the same as they were previously
            string displayWpm = TypingModel.SelectedDifficulty == Difficulty.Easy
                ? CurrentUser.CurrentUser.Statistics.AverageWpm.ToString()
                : TypingModel.AverageTypingSpeed;
            string displayAcc = TypingModel.SelectedDifficulty == Difficulty.Easy
                ? CurrentUser.CurrentUser.Statistics.AverageAccuracy.ToString(CultureInfo.InvariantCulture)
                : TypingModel.AverageAccuracy;

            double displayHighestWpm = TypingModel.SelectedDifficulty == Difficulty.Easy
                ? CurrentUser.CurrentUser.Statistics.HighestWPM
                : TypingModel.HighestSpeed;
            double displayHoursSpent = TypingModel.SelectedDifficulty == Difficulty.Easy
                ? CurrentUser.CurrentUser.Statistics.HoursSpent
                : TypingModel.SecondsSpent / 3600;
            int displayTypedWords = TypingModel.SelectedDifficulty == Difficulty.Easy
                ? CurrentUser.CurrentUser.Statistics.TypedWordsTotal
                : TypingModel.TypedWordsTotal;
            
            // update the tracking of user document progress
            XmlHandler.UpdateDocuments(CurrentUser.CurrentUser.Name, TypingModel.Document.Name,
                (TypingModel.PageNumber + 1).ToString(), displayAcc);

            // Updates the user's averages
            XmlHandler.UpdateUserStatistics(CurrentUser.CurrentUser.Name, "AverageWPM",
                double.Parse(displayWpm).ToString(CultureInfo.InvariantCulture));
            XmlHandler.UpdateUserStatistics(CurrentUser.CurrentUser.Name, "AverageAccuracy",
                double.Parse(displayAcc).ToString(CultureInfo.InvariantCulture));

            // Updates user tallies
            XmlHandler.UpdateUserStatistics(CurrentUser.CurrentUser.Name, "HighestWPM",
                displayHighestWpm.ToString(CultureInfo.InvariantCulture));
            XmlHandler.UpdateUserStatistics(CurrentUser.CurrentUser.Name, "HoursSpent",
                displayHoursSpent.ToString(CultureInfo.InvariantCulture));
            XmlHandler.UpdateUserStatistics(CurrentUser.CurrentUser.Name, "TypedWordsTotal",
                displayTypedWords.ToString(CultureInfo.InvariantCulture));
                        
            AchievementHandler.FinishPageAchievements(CurrentUser, int.Parse(displayWpm), double.Parse(displayAcc, CultureInfo.InvariantCulture));

            // Adding a new daily record to the user
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            XmlHandler.AddDailyRecordToUser(CurrentUser.CurrentUser.Name, date, int.Parse(displayWpm), double.Parse(displayAcc, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Checking if the user can go to the next word depending if the word is written correctly or not
        /// </summary>
        /// <param name="word">the word which the user has typed</param>
        /// <returns>a boolean determining if the user can proceed | return true if yes, false if not</returns>
        private bool CanGoToNextWord(string word)
        {
            if (InputString.Length <= 0)
            {
                return false;
            }

            // check if you're at the last word to see if the space is needed to continue
            // important otherwise the text will proceed without a space needed
            var curNum = TypingModel.GetNumberOfWords();
            if (TypingModel.CurrentWordIndex == curNum)
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

            return false;
        }

        /// <summary>
        /// Checks if the user has failed typing this page (Only for hard and extreme difficulty
        /// </summary>
        private void CheckIfFailed()
        {
            if (TypingModel.SelectedDifficulty == Difficulty.Hard ||
                TypingModel.SelectedDifficulty == Difficulty.Extreme)
            {
                // fail the user if time runs out
                if (TypingModel.TimeCounter == 0)
                {
                    TypingModel.Alive = false;
                    TypingModel.TypingTimer.Stop();

                    //Custom messagebox
                    var res = Xceed.Wpf.Toolkit.MessageBox.Show(
                        "You ran out of time :(",
                        "OK",
                        MessageBoxButton.OK
                    );
                            
                    if ("OK" == res.ToString())
                    {                        
                        NavigateHomeCommand.Execute(null);
                    }
                }
            }
        }
    }
}