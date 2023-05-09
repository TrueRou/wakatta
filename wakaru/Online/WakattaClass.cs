using Newtonsoft.Json;
using Quartz;
using System.Threading.Tasks;
using wakaru.Quartz;

namespace wakaru.Online
{
    public partial class WakattaClass
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
        public string TimeDurationString { get { return TimeDuration + " 分钟"; } }
        public string TimeString { get { return Utils.GetTimeString(TimeHour, TimeMinute, TimeDuration); } }

        public async Task ScheduleJobs()
        {
            var overTime = Utils.DelayDatetime(TimeHour, TimeMinute, TimeDuration);
            await ScheduleManager.AddRingCornJob(JobBuilder.Create<ClassBeginRingJob>().Build(), TimeHour, TimeMinute);
            await ScheduleManager.AddRingCornJob(JobBuilder.Create<ClassOverRingJob>().Build(), overTime.Hour, overTime.Minute);
        }
    }
}
