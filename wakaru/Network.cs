using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Threading;
using RestSharp.Serializers.NewtonsoftJson;
using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace wakaru
{
    internal static class Network
    {
        static readonly RestClient Client = new(Configuration.Instance.APIUrl, configureSerialization: s => s.UseNewtonsoftJson());
        public static WakattaClient? CurrentClient { get; set; }
        public static Queue<string> CurrentMessageQueue { get; set; } = new Queue<string>();

        private static string GenHarewareID()
        {
            var hardwareID = Guid.NewGuid().ToString();
            Configuration.Instance.HardwareID = hardwareID;
            Configuration.Save();
            return hardwareID;
        }
        public async static Task Connect()
        {
            var hardwareId = Configuration.Instance.HardwareID ?? GenHarewareID();
            try { CurrentClient = await Client.GetJsonAsync<WakattaClient>("client", new { hardware_id = hardwareId }); } catch { }
        }
        private async static Task CheckConnection()
        {
            if (CurrentClient == null)
            {
                await Connect();
                return;
            }
        }
        public async static void Heartbeat()
        {
            await CheckConnection();
            var response = await Client.GetJsonAsync<WakattaHeartbeat>("client/heartbeat", new { client_id = CurrentClient.Id });
            if (response == null) return;
            if (response.NeedRefresh)
            {
                await Connect();
                // Refresh Scheduler
            }
            foreach (var item in response.Messages)
            {
                CurrentMessageQueue.Enqueue(item.ToString());
            }
        }
    }
}
