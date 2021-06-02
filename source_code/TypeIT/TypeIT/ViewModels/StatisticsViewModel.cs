using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class StatisticsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ChartValues<int> WpmValues { get; set; }
        public ChartValues<double> AccuracyValues { get; set; }
        public List<string> Dates { get; set; }
        public UserStore currentUser { get; set; }

        public StatisticsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            
            //Current user
            currentUser = userStore;

            WpmValues = new ChartValues<int>
            {
                //Here we will load the data from the user' save profile and extract wpm
                56,
                72,
                26,
                66,
                110,
                92,
                78,
                66,
                59,
                55,
                100,
                97,
                87,
                72,
                59,
                66
            };

            AccuracyValues = new ChartValues<double>
            {
                96.1,
                92.2,
                96.3,
                96.4,
                91.5,
                92.6,
                98.7,
                96.8,
                99.9,
                95.1,
                90.2,
                97.3,
                97.4,
                92.5,
                99.6,
                96.7
            };

            Dates = new List<string>
            {
                //Here we will load the data from the user' save profile and extract dates
                "28/05/21",
                "29/05/21",
                "30/05/21",
                "31/05/21",
                "01/06/21",
                "02/06/21",
                "03/06/21",
                "04/06/21",
                "05/06/21",
                "06/06/21",
                "07/06/21",
                "08/06/21",
                "09/06/21",
                "10/06/21",
                "11/06/21",
                "12/06/21"
            };
        }
    }
}
