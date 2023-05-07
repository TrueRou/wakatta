using Newtonsoft.Json;
using Quartz;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wakaru.Quartz;
using wakaru.Views;

namespace wakaru.Online
{
    public class WakattaClient
    {
        public static WakattaClient? CurrentClient;

        private readonly static string Version = "wakaru 1.0.0 (.net6.0 Windows)";

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "identifier")]
        public string Identifier { get; set; } = "";
        [JsonProperty(PropertyName = "class_begin_ringtone_filename")]
        public string ClassBeginRingtone { get; set; } = "default_class_begin.wav";
        [JsonProperty(PropertyName = "class_over_ringtone_filename")]
        public string ClassOverRingtone { get; set; } = "default_class_over.wav";
        [JsonProperty(PropertyName = "classes")]
        public List<WakattaClass>? Classes { get; set; }
        [JsonProperty(PropertyName = "subscribe_schedule")]
        public ScheduleBase? SubscribeSchedule { get; set; }

        public static RestClient CreateWebClient() 
        {
            return new RestClient(Configuration.Instance.APIUrl, configureSerialization: s => s.UseNewtonsoftJson());
        }

        public static async Task Create()
        {
            using var client = CreateWebClient();
            var hardwareId = Configuration.Instance.HardwareID ?? Utils.CreateHardwareID();
            CurrentClient = await client.GetJsonAsync<WakattaClient>("client/create", new { hardware_id = hardwareId, version = Version }).ConfigureAwait(false);
            await WakattaHeartbeat.ScheduleJob();
            await (CurrentClient?.RefreshClasses() ?? Task.CompletedTask);
            OnlinePanel.UpdateStatus();
            ApplyStatus();
        }

        public async Task RefreshClasses()
        {
            using var client = CreateWebClient();
            Classes = await client.GetJsonAsync<List<WakattaClass>>("client/class", new { client_id = Id });
            await (WakattaSchedule.Instance?.Refresh() ?? Task.CompletedTask);
            ApplyStatus();
        }

        public static void ApplyStatus()
        {
            if (CurrentClient != null && CurrentClient.Classes != null)
            {
                var index = Utils.IndexClass(CurrentClient.Classes);
                if (index == -1) // No class matched, maybe there is no class now or class over
                {
                    StatusPanel.UpdateStatus(StatusPanel.Status.IDLE);
                    ProfilePanel.ClearTime();
                    return;
                };
                var clazz = CurrentClient.Classes[index];
                StatusPanel.UpdateStatus(StatusPanel.Status.IN_CLASS);
                ProfilePanel.UpdateTime(clazz.TimeHour, clazz.TimeMinute, clazz.TimeDuration);
                WakattaSchedule.SetSelectedIndex(index);
            }
        }
    }

    public class ScheduleBase
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; } = string.Empty;
    }
}
