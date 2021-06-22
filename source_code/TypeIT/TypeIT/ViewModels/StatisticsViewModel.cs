using LiveCharts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    internal class StatisticsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public UserStore CurrentUser { get; set; }
        public ChartValues<int> WpmValues { get; set; }
        public ChartValues<double> AccuracyValues { get; set; }
        public ObservableCollection<string> Dates { get; set; }


        public StatisticsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));

            //Current user
            CurrentUser = userStore;

            //Collection for the charts
            WpmValues = (ChartValues<int>)XmlHandler.GetValues(CurrentUser.CurrentUser.Name)[0];
            AccuracyValues = (ChartValues<double>)XmlHandler.GetValues(CurrentUser.CurrentUser.Name)[1];
            Dates = (ObservableCollection<string>)XmlHandler.GetValues(CurrentUser.CurrentUser.Name)[2];

        }
    }
}
