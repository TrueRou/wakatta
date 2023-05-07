using Newtonsoft.Json;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
using wakaru.Quartz;

namespace wakaru.Online
{
    /// <summary>
    /// WakattaSchedule.xaml 的交互逻辑
    /// </summary>
    public partial class WakattaSchedule : UserControl
    {
        public static WakattaSchedule? Instance;

        public ObservableCollection<WakattaClass> ClassList = new();

        public WakattaSchedule()
        {
            Instance = this;
            InitializeComponent();
            SchedulerListView.ItemsSource = ClassList;
            SetSelectedIndex(Utils.IndexClass(ClassList));
        }

        public async Task Refresh()
        {
            if (WakattaClient.CurrentClient?.Classes != null)
            {
                await ScheduleManager.ResetGroup("ring");
                foreach (var clazz in WakattaClient.CurrentClient.Classes)
                {
                    await clazz.ScheduleJobs();
                }
                Instance?.Dispatcher.Invoke(() =>
                {
                    ClassList.Clear();
                    foreach (var clazz in WakattaClient.CurrentClient.Classes)
                    {
                        ClassList.Add(clazz);
                    }
                    SchedulerListView.ItemsSource = ClassList;
                });
            }
        }

        public static void SetSelectedIndex(int index) 
        {
            Instance?.Dispatcher.Invoke(() =>
            {
                if (index != -1)
                    Instance.SchedulerListView.SelectedItem = WakattaClient.CurrentClient?.Classes?[index];
            });
        }
    }
}
