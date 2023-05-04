using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    /// WakattaClass.xaml 的交互逻辑
    /// </summary>
    public partial class WakattaClass : UserControl
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; } = "";
        [JsonProperty(PropertyName = "time_hour")]
        public int TimeHour { get; set; }
        [JsonProperty(PropertyName = "time_minute")]
        public int TimeMinute { get; set; }
        [JsonProperty(PropertyName = "time_duration")]
        public int TimeDuration { get; set; }
        public WakattaClass()
        {
            InitializeComponent();
        }

        public async Task ScheduleJobs()
        {
            var overTime = OnlineUtils.DelayDatetime(TimeHour, TimeMinute, TimeDuration);
            await ScheduleManager.AddCornJob(JobBuilder.Create<ClassBeginRingJob>().Build(), TimeHour, TimeMinute);
            await ScheduleManager.AddCornJob(JobBuilder.Create<ClassBeginRingJob>().Build(), overTime.Hour, overTime.Minute);
        }
    }
}
