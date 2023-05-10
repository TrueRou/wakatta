using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using wakaru.Online;

namespace wakaru.Quartz
{
    internal class VITSManager
    {
        private static VITSObject vitsObject = new() { Enabled = false, Entrypoint = "" };
        private static readonly string classBeginPrefix = "同学们   上课了   请为下节{0}课做好准备";

        public static async Task PrepareVITS()
        {
            using var client = WakattaClient.CreateWebClient();
            var obj = await client.GetJsonAsync<VITSObject>("vits/entrypoint");
            vitsObject = obj ?? vitsObject;
        }

        public static async Task RefreshId()
        {
            if (WakattaClient.CurrentClient == null) return;
            using var client = WakattaClient.CreateWebClient();
            var obj = await client.GetJsonAsync<VITSCharacter>("vits/id", new { client_id = WakattaClient.CurrentClient.Id });
            if (obj != null) WakattaClient.CurrentClient.VITSId = obj.CharacterId;
        }

        public static async Task PlaySound(string content)
        {
            if (!vitsObject.Enabled) return;
            if (WakattaClient.CurrentClient == null) return;
            var vitsId = WakattaClient.CurrentClient.VITSId;
            using HttpClient client = new();
            string url = vitsObject.Entrypoint + $"voice?id={vitsId}&text={content}";
            HttpResponseMessage response = await client.GetAsync(url);
            MemoryStream memoryStream = new();
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);

            SoundPlayer player = new(memoryStream);
            player.Play();

            response.Dispose();
            memoryStream.Dispose();
        }

        public static async Task PlaySoundFormat(string content)
        {
            var str = string.Format(classBeginPrefix, content);
            await PlaySound(str);
        }
    }

    public class VITSObject
    {
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [JsonProperty(PropertyName = "entrypoint")]
        public string Entrypoint { get; set; } = string.Empty;
    }

    public class VITSCharacter
    {
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int CharacterId { get; set; }
    }
}
