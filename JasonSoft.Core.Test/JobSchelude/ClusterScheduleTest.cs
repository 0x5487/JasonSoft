using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using JasonSoft.Components.Logging;
using JasonSoft.Components.Quartz;
using JasonSoft.Components.Quartz.Impl;

using Xunit;

namespace JasonSoft.Tests.JobSchelude
{

    public class ClusterScheduleTest
    {
        public ClusterScheduleTest()
        {
            NameValueCollection properties = new NameValueCollection();

            properties["quartz.scheduler.instanceName"] = "UGuardCore";
            properties["quartz.scheduler.instanceId"] = "Jason1";
            properties["quartz.threadPool.type"] = "JasonSoft.Components.Quartz.Simpl.SimpleThreadPool, JasonSoft.Core";
            properties["quartz.threadPool.threadCount"] = "1";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.jobStore.misfireThreshold"] = "60000";
            properties["quartz.jobStore.type"] = "JasonSoft.Components.Quartz.Impl.AdoJobStore.JobStoreTX, JasonSoft.Core";
            properties["quartz.jobStore.useProperties"] = "false";
            properties["quartz.jobStore.dataSource"] = "default";
            properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            properties["quartz.jobStore.clustered"] = "true";
            // if running MS SQL Server we need this
            properties["quartz.jobStore.lockHandler.type"] = "JasonSoft.Components.Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, JasonSoft.Core";
            properties["quartz.dataSource.default.connectionString"] = "Server=(local);Database=ServiceDB;Trusted_Connection=True;";
            properties["quartz.dataSource.default.provider"] = "SqlServer-20";

            //// First we must get a reference to a scheduler
            sf = new StdSchedulerFactory(properties);
            sched = sf.GetScheduler();
        }

        private static ILog _log = LogManager.GetLogger(typeof(ClusterScheduleTest));
        private ISchedulerFactory sf;
        private IScheduler sched;

        [Fact]
        public void createJob()
        {
            _log.Info("------- Initialization Complete -----------");
            Console.WriteLine("ddd");

            Thread.Sleep(5 * 1000);
            //if (true)
            //{
            //    CleanUp(sched);
            //}

            //_log.Info("------- Initialization Complete -----------");
            //Console.WriteLine("dd");

            //if (true)
            //{
            //    _log.Info("------- Scheduling Jobs ------------------");

            //    string schedId = sched.SchedulerInstanceId;

            //    int count = 1;

            //    JobDetail job = new JobDetail("job_" + count, schedId, typeof(SimpleJob));
            //    job.RequestsRecovery = false;
            //    SimpleTrigger trigger = new SimpleTrigger("trig_" + count, schedId, -1, 5000L);
            //    trigger.MisfireInstruction = MisfireInstruction.SimpleTrigger.RescheduleNextWithExistingCount;
            //    trigger.StartTimeUtc = DateTime.UtcNow.AddMilliseconds(1000);
            //    sched.ScheduleJob(job, trigger);
            //    _log.Info(string.Format("{0} will run at: {1} and repeat: {2} times, every {3} seconds", job.FullName, trigger.GetNextFireTimeUtc(), trigger.RepeatCount, trigger.RepeatInterval / 1000));

            //    count++;
            //    job = new JobDetail("job_" + count, schedId, typeof(SimpleJob));
            //    job.RequestsRecovery = false;
            //    trigger = new SimpleTrigger("trig_" + count, schedId, 20, 5000L);
            //    trigger.MisfireInstruction = MisfireInstruction.SimpleTrigger.RescheduleNextWithExistingCount;
            //    trigger.StartTimeUtc = DateTime.UtcNow.AddMilliseconds(2000);
            //    sched.ScheduleJob(job, trigger);
            //    _log.Info(string.Format("{0} will run at: {1} and repeat: {2} times, every {3} seconds", job.FullName, trigger.GetNextFireTimeUtc(), trigger.RepeatCount, trigger.RepeatInterval / 1000));


            //}

            //// jobs don't start firing until start() has been called...
            //_log.Info("------- Starting Scheduler ---------------");
            //sched.Start();
            //_log.Info("------- Started Scheduler ----------------");

            //_log.Info("------- Waiting for one hour... ----------");

            //Thread.Sleep(TimeSpan.FromHours(1));


            //_log.Info("------- Shutting Down --------------------");
            //sched.Shutdown();
            //_log.Info("------- Shutdown Complete ----------------");
        }

        public virtual void CleanUp(IScheduler inScheduler)
        {
            _log.Warn("***** Deleting existing jobs/triggers *****");

            // unschedule jobs
            string[] groups = inScheduler.TriggerGroupNames;
            for (int i = 0; i < groups.Length; i++)
            {
                string[] names = inScheduler.GetTriggerNames(groups[i]);
                for (int j = 0; j < names.Length; j++)
                {
                    inScheduler.UnscheduleJob(names[j], groups[i]);
                }
            }

            // delete jobs
            groups = inScheduler.JobGroupNames;
            for (int i = 0; i < groups.Length; i++)
            {
                string[] names = inScheduler.GetJobNames(groups[i]);
                for (int j = 0; j < names.Length; j++)
                {
                    inScheduler.DeleteJob(names[j], groups[i]);
                }
            }
        }
    }
}
