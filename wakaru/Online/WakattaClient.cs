﻿using Newtonsoft.Json;
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

        public static async Task Create()
        {
            var hardwareId = Configuration.Instance.HardwareID ?? Utils.CreateHardwareID();
            CurrentClient = await WebClient.GetJsonAsync<WakattaClient>("client", new { hardware_id = hardwareId, version=Version });
            await ScheduleManager.ResetClientScheduler();
            await WakattaHeartbeat.ScheduleJob();
            await (CurrentClient?.RefreshClasses() ?? Task.CompletedTask);
        }

        public async Task RefreshClasses()
        {
            Classes = await WebClient.GetJsonAsync<List<WakattaClass>>("client", new { client_id = Id });
            await (WakattaSchedule.Instance?.Refresh() ?? Task.CompletedTask);
        }
    }
}
