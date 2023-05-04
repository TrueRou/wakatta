using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wakaru.Quartz
{
    internal static class ScheduleManager
    {
        private static StdScheduler? RingScheduler { get; set; }
        private static StdScheduler? ClientScheduler { get; set; }
        public static async Task CreateScheduler()
        {
            var schedulerFactory = new StdSchedulerFactory();
            RingScheduler = (StdScheduler?) await schedulerFactory.GetScheduler();
            ClientScheduler = (StdScheduler?)await schedulerFactory.GetScheduler();
            await (RingScheduler?.Start() ?? Task.CompletedTask);
            await (ClientScheduler?.Start() ?? Task.CompletedTask);
        }

        public static async Task AddRingCornJob(IJobDetail jobDetail, int hour, int minute)
        {
            ITrigger trigger = TriggerBuilder.Create()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, minute))
                    .Build();
            await (RingScheduler?.ScheduleJob(jobDetail, trigger) ?? Task.CompletedTask);
        }

        public static async Task AddClientIntervalJob(IJobDetail jobDetail, int interval)
        {
            ITrigger trigger = TriggerBuilder.Create()
                    .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(interval))
                    .Build();
            await (ClientScheduler?.ScheduleJob(jobDetail, trigger) ?? Task.CompletedTask);
        }

        public static async Task ResetRingScheduler()
        {
            await (RingScheduler?.Clear() ?? Task.CompletedTask);
        }

        public static async Task ResetClientScheduler()
        {
            await (ClientScheduler?.Clear() ?? Task.CompletedTask);
        }
    }
}
