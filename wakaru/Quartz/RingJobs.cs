using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using wakaru.Online;
using wakaru.Views;

namespace wakaru.Quartz
{
    public abstract class RingJob : IJob
    {
        private static readonly string SoundFolder = Path.Combine(Directory.GetCurrentDirectory(), "ringtones");
        public abstract Task Execute(IJobExecutionContext context);
        public static void PlaySound(string fileName)
        {
            var filePath = Path.Combine(SoundFolder, fileName);
            new SoundPlayer(filePath).Play();
        }
    }
    public class ClassBeginRingJob : RingJob
    {
        public override Task Execute(IJobExecutionContext context)
        {
            int hour = (int)context.MergedJobDataMap.Get("hour");
            int minute = (int)context.MergedJobDataMap.Get("minute");
            int duration = (int)context.MergedJobDataMap.Get("duration");
            if (WakattaClient.CurrentClient != null)
            {
                PlaySound(WakattaClient.CurrentClient.ClassBeginRingtone);
                StatusPanel.UpdateStatus(StatusPanel.Status.IN_CLASS);
                ProfilePanel.UpdateTime(hour, minute, duration);
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
            }
            return Task.CompletedTask;
        }
    }
}
