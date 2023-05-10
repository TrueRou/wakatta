using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wakaru.Online;
using wakaru.Views;

namespace wakaru.Quartz
{
    public abstract class RingJob : IJob
    {
        public static readonly string SoundFolder = Path.Combine(Directory.GetCurrentDirectory(), "ringtones");
        public abstract Task Execute(IJobExecutionContext context);
        public static void PlaySound(string fileName)
        {
            var filePath = Path.Combine(SoundFolder, fileName);
            new SoundPlayer(filePath).Play();
        }

        public static void PlaySoundSync(string fileName)
        {
            var filePath = Path.Combine(SoundFolder, fileName);
            new SoundPlayer(filePath).PlaySync();
        }
    }
    public class ClassBeginRingJob : RingJob
    {
        public override Task Execute(IJobExecutionContext context)
        {
            if (WakattaClient.CurrentClient != null)
            {
                var label = context.Get("label")?.ToString();
                Task.Run(async () => {
                    PlaySoundSync(WakattaClient.CurrentClient.ClassBeginRingtone);
                    if (label != null) await VITSManager.PlaySoundFormat(label);
                });
                WakattaClient.ApplyStatus();
            }
            return Task.CompletedTask;
        }
    }

    public class ClassOverRingJob : RingJob
    {
        public override Task Execute(IJobExecutionContext context)
        {
            if (WakattaClient.CurrentClient != null)
            {
                PlaySound(WakattaClient.CurrentClient.ClassOverRingtone);
                StatusPanel.UpdateStatus(StatusPanel.Status.CLASS_OVER);
                ProfilePanel.ClearTime();
                WakattaSchedule.SetSelectedIndex(-1);
            }
            return Task.CompletedTask;
        }
    }
}
