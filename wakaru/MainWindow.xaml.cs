using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using wakaru.Online;
using wakaru.Quartz;

namespace wakaru
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly MainWindow Instance = (MainWindow) Application.Current.MainWindow;

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(async () => {
                await ScheduleManager.CreateScheduler();
                await WakattaClient.Create();
            });
            UpdateProgressBar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Instance.Close();
        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Instance.DragMove();
        }

        public static void UpdateProgressBar()
        {
            var color = Configuration.Instance.UseLocal ? Colors.IndianRed : Colors.MediumPurple;
            Instance.ProgressBarMain.Background = new SolidColorBrush(color);
        }
    }
}
