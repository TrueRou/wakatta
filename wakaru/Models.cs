using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wakaru
{
    public class WakattaClass
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
    }

    public class WakattaSchedule
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; } = "";
    }

    public class WakattaHeartbeat
    {
        [JsonProperty(PropertyName = "messages")]
        public List<string> Messages { get; set; } = new List<string>();
        [JsonProperty(PropertyName = "need_refresh")]
        public bool NeedRefresh { get; set; }
    }

    public class WakattaClient
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "identifier")]
        public string Identifier { get; set; } = "";
        [JsonProperty(PropertyName = "classes")]
        public List<WakattaClass>? Classes { get; set; }
        [JsonProperty(PropertyName = "subscribe_schedule")]
        public WakattaSchedule? SubscribeSchedule { get; set; }
    }
}
