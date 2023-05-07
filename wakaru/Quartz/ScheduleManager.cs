using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wakaru.Quartz
{
    internal static class ScheduleManager
    {
        public static StdScheduler? Scheduler { get; set; }
        public static async Task CreateScheduler()
        {
            Scheduler = (StdScheduler?) await new StdSchedulerFactory().GetScheduler();
            await (Scheduler?.Start() ?? Task.CompletedTask);
        }

        public static async Task AddRingCornJob(IJobDetail jobDetail, int hour, int minute)
        {
            ITrigger trigger = TriggerBuilder.Create()
                    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, minute))
                    .WithIdentity(Guid.NewGuid().ToString(), "ring")
                    .Build();
            await (Scheduler?.ScheduleJob(jobDetail, trigger) ?? Task.CompletedTask);
        }

        public static async Task AddClientIntervalJob(IJobDetail jobDetail, int interval)
        {
            ITrigger trigger = TriggerBuilder.Create()
                    .WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(interval))
                    .WithIdentity(Guid.NewGuid().ToString(), "client")
                    .Build();
            await (Scheduler?.ScheduleJob(jobDetail, trigger) ?? Task.CompletedTask);
        }

        public static async Task ResetGroup(string group)
        {
            if (Scheduler == null) return;
            var jobKeys = await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
            await Scheduler.DeleteJobs(jobKeys.ToList());
        }
    }
}
