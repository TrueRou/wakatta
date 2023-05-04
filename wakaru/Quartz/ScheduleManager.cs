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
        private static StdScheduler? Scheduler { get; set; }
        public static async void CreateScheduler()
        {
            var schedulerFactory = new StdSchedulerFactory();
            Scheduler = (StdScheduler?) await schedulerFactory.GetScheduler();
        }

        public static async Task AddCornJob(IJobDetail jobDetail, int hour, int minute)
        {
            ITrigger trigger = TriggerBuilder.Create()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, minute))
                    .Build();
            await (Scheduler?.ScheduleJob(jobDetail, trigger) ?? Task.CompletedTask);
        }

        public static async Task ResetScheduler()
        {
            await (Scheduler?.Clear() ?? Task.CompletedTask);
        }
    }
}
