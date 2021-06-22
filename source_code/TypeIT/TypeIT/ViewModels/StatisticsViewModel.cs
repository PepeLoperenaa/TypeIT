using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    class StatisticsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand NavigateHomeCommand { get; }
        //public ChartValues<int> WpmValues { get; set; }
        //public ChartValues<double> AccuracyValues { get; set; }
        //public ObservableCollection<string> Dates { get; set; }
        public UserStore currentUser { get; set; }

        private ChartValues<int> _wpmValues;

        // used to calculate accuracy for the page
        public ChartValues<int> WpmValues
        {
            get => _wpmValues;
            set
            {
                _wpmValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WpmValues)));
            }
        }

        private ChartValues<double> _accuracyValues;

        // used to calculate accuracy for the page
        public ChartValues<double> AccuracyValues
        {
            get => _accuracyValues;
            set
            {
                _accuracyValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccuracyValues)));
            }
        }

        private ObservableCollection<string> _dates;

        // used to calculate accuracy for the page
        public ObservableCollection<string> Dates
        {
            get => _dates;
            set
            {
                _dates = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dates)));
            }
        }

        public StatisticsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            
            //Current user
            currentUser = userStore;

            WpmValues = (ChartValues<int>)XmlHandler.GetValues(currentUser.CurrentUser.Name)[0];
            AccuracyValues = (ChartValues<double>)XmlHandler.GetValues(currentUser.CurrentUser.Name)[1];
            Dates = (ObservableCollection<string>)XmlHandler.GetValues(currentUser.CurrentUser.Name)[2];

        }      
    }
}
