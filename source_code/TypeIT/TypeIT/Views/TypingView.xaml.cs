using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TypeIT.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class TypingView : UserControl
    {
        public TypingView()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += ((sender, e) =>
            {
                Content.Height += 10;

                if (ScollViewer.VerticalOffset == ScollViewer.ScrollableHeight)
                {
                    ScollViewer.ScrollToEnd();
                }
            });
            timer.Start();
        }
    }
}
