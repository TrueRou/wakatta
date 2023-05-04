using Newtonsoft.Json;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wakaru.Online
{
    class HeartbeatPacket 
    {
        [JsonProperty(PropertyName = "packet_id")]
        public int PacketId { get; set; }
        [JsonProperty(PropertyName = "payload")]
        public string Payload { get; set; } = string.Empty;
    }
    class WakattaHeartbeat : IJob
    {
        public static async Task HandlePacket(HeartbeatPacket packet)
        {
            switch (packet.PacketId)
            {
                case (int) Packets.MESSAGE: break;
                case (int)Packets.REFRESH_SCHEDULE:
                    await (WakattaClient.CurrentClient?.RefreshClasses() ?? Task.CompletedTask);
                    break;
                case (int) Packets.RECONNECT: 
                    await WakattaClient.Create();
                    break;
            }
        }
        public async Task Execute(IJobExecutionContext context)
        {
            if (WakattaClient.CurrentClient != null)
            {
                var packets = await WakattaClient.WebClient.GetJsonAsync<List<HeartbeatPacket>>("client/heartbeat", new { client_id = WakattaClient.CurrentClient.Id });
                if (packets != null)
                {
                    foreach (var item in packets)
                    {
                        await HandlePacket(item);
                    }
                }
            }
        }
    }
}
