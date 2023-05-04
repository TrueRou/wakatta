using Newtonsoft.Json;
using Quartz;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakaru.Quartz;

namespace wakaru.Online
{
    class WakattaClient
    {
        public static WakattaClient? CurrentClient;

        public readonly static RestClient WebClient = new(Configuration.Instance.APIUrl, configureSerialization: s => s.UseNewtonsoftJson());

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "identifier")]
        public string Identifier { get; set; } = "";
        [JsonProperty(PropertyName = "classes")]
        public List<WakattaClass>? Classes { get; set; }

        public static async Task Create()
        {
            var hardwareId = Configuration.Instance.HardwareID ?? OnlineUtils.CreateHardwareID();
            CurrentClient = await WebClient.GetJsonAsync<WakattaClient>("client", new { hardware_id = hardwareId });
            await ScheduleManager.ResetClientScheduler();
            await WakattaHeartbeat.ScheduleJob();
        }

        public async Task RefreshClasses()
        {
            Classes = await WebClient.GetJsonAsync<List<WakattaClass>>("client", new { client_id = Id });
            WakattaSchedule.Instance?.Refresh();
        }
    }
}
