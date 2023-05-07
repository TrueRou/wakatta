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
using wakaru.Online;

namespace wakaru.Views
{
    /// <summary>
    /// ProfilePanel.xaml 的交互逻辑
    /// </summary>
    public partial class ProfilePanel : UserControl
    {
        private static ProfilePanel? Instance;
        private static DateTime OverDateTime = DateTime.Now;
        public ProfilePanel()
        {
            Instance = this;
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new() { Interval = new TimeSpan(0, 0, 0, 1, 0) };
            dispatcherTimer.Tick += new EventHandler((o, e) => {
                Dispatcher.BeginInvoke(new Action(() => {
                    TimeNow.Text = DateTime.Now.ToString();
                    TimeInterval.Text = new TimeSpan(OverDateTime.Ticks - DateTime.Now.Ticks).ToString(@"hh\:mm\:ss");
                }));
            });
            ClearTime();
            dispatcherTimer.Start();
        }

        public static void UpdateTime(int hour, int minute, int duration)
        {
            
            Instance?.Dispatcher.BeginInvoke(new Action(() =>
            {
                Instance.LabelInterval.Visibility = Visibility.Visible;
                Instance.LabelNext.Visibility = Visibility.Visible;
                Instance.TimeNext.Visibility = Visibility.Visible;
                Instance.TimeInterval.Visibility = Visibility.Visible;
                OverDateTime = Utils.DelayDatetime(hour, minute, duration);
            }));
        }

        public static void ClearTime()
        {
            Instance?.Dispatcher.BeginInvoke(new Action(() =>
            {
                Instance.LabelInterval.Visibility = Visibility.Hidden;
                Instance.LabelNext.Visibility = Visibility.Hidden;
                Instance.TimeNext.Visibility = Visibility.Hidden;
                Instance.TimeInterval.Visibility = Visibility.Hidden;
                OverDateTime = DateTime.Now;
            }));
        }
    }
}
