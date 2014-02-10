using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using JasonSoft.Components.Logging;
using JasonSoft.Components.Quartz;
using JasonSoft.Components.Quartz.Impl;
using Xunit;

namespace JasonSoft.Tests.JobSchelude
{
    public class RunJob
    {
        private static ILog _log = LogManager.GetLogger(typeof(RunJob));

        public RunJob()
        {
 
        }

        [Fact]
        public void Running()
        {
            NameValueCollection properties = new NameValueCollection();

            properties["quartz.scheduler.instanceName"] = "UGuardCore";
            properties["quartz.scheduler.instanceId"] = "Jason2";
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

            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();

            // jobs don't start firing until start() has been called...
            _log.Info("------- Starting Scheduler ---------------");
            sched.Start();
            _log.Info("------- Started Scheduler ----------------");

            _log.Info("------- Waiting for one hour... ----------");

            Thread.Sleep(TimeSpan.FromHours(1));


            _log.Info("------- Shutting Down --------------------");
            sched.Shutdown();
            _log.Info("------- Shutdown Complete ----------------");
        }
    }
}
