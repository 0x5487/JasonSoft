using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using JasonSoft.Services.JobSchedule.ClientProxy;
using JasonSoft.Services.JobSchelude.Data;
using JasonSoft.Services.JobSchelude;
using JasonSoft.Components.Quartz;
using JasonSoft.Components.Quartz.Impl;
using Xunit;
using Daily=JasonSoft.Services.JobSchelude.Daily;
using Job=JasonSoft.Services.JobSchelude.Job;
using JobHistory=JasonSoft.Services.JobSchelude.JobHistory;
using JobState=JasonSoft.Services.JobSchelude.JobState;
using OnceDailyFrequency=JasonSoft.Services.JobSchelude.OnceDailyFrequency;
using RecurvedTrigger=JasonSoft.Services.JobSchelude.RecurvedTrigger;
using RepeatedDailyFrequency=JasonSoft.Services.JobSchelude.RepeatedDailyFrequency;
using State=JasonSoft.Services.JobSchelude.State;
using Trigger=JasonSoft.Components.Quartz.Trigger;


namespace JasonSoft.Tests.Schelude
{

    public class JobScheduleTestCase
    {
        private ScheduleService _scheduleService;


        public JobScheduleTestCase()
        {
            _scheduleService = new ScheduleService();
        }

        [Fact]
        public void MultipleJobs()
        {
            _scheduleService.ClearJobsAndTriggers();
            _scheduleService.ClearJobHistory();

            Job job = new Job();
            job.Name = "MultipleJob1";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";
            Dictionary<String, object> arguments = new Dictionary<string, object>();
            arguments.Add("JobType", "Job1");
            job.Argument = arguments;
            _scheduleService.CreateJob(job);
            _scheduleService.RunJobOnceNow("MultipleJob1", "TestJobGroup");

            job = new Job();
            job.Name = "MultipleJob2";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";
            arguments = new Dictionary<string, object>();
            arguments.Add("JobType", "Job2");
            job.Argument = arguments;
            _scheduleService.CreateJob(job);
            _scheduleService.RunJobOnceNow("MultipleJob2", "TestJobGroup");

            Boolean repeat = true;
            
            while (repeat)
            {
                JobState state1 = _scheduleService.GetStateByJob("MultipleJob1", "TestJobGroup");
                JobState state2 = _scheduleService.GetStateByJob("MultipleJob2", "TestJobGroup");

                Thread.Sleep(8 * 1000);
            }
            
        }

        [Fact]
        public void ResetTest()
        {
            _scheduleService.ClearJobsAndTriggers();
            _scheduleService.ClearJobHistory();

            Job job = new Job();
            job.Name = "ResetJob";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";

            RecurvedTrigger recurvedTrigger = new RecurvedTrigger();
            recurvedTrigger.Name = "ResetTrigger";
            recurvedTrigger.Group = "TestTriggerGroup";
            recurvedTrigger.JobName = "ResetJob";
            recurvedTrigger.JobGroupName = "TestJobGroup";

            recurvedTrigger.Frequency = new Daily() { RepeatedDays = 1 };

            DateTime start = new DateTime(1900, 1, 1, 1, 0, 0);
            DateTime end = new DateTime(1900, 1, 1, 14, 0, 0);
            recurvedTrigger.DailyFrequency = new RepeatedDailyFrequency() { StartingAtUTC = start, EndingAtUTC = end, RepeatedMinutes = 1 };

            recurvedTrigger.StartDateUTC = new DateTime(2008, 1, 1).ToUniversalTime();
            _scheduleService.ScheduleJob(job, recurvedTrigger);

            Thread.Sleep(90*1000);

            _scheduleService.ClearJobsAndTriggers();
            _scheduleService.ClearJobHistory();

            job = new Job();
            job.Name = "RecurvedTriggerWithRepeatedDailyFrequencyJob";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";

            recurvedTrigger = new RecurvedTrigger();
            recurvedTrigger.Name = "RecurvedTriggerWithRepeatedDailyFrequencyTrigger";
            recurvedTrigger.Group = "TestTriggerGroup";
            recurvedTrigger.JobName = "RecurvedTriggerWithRepeatedDailyFrequencyJob";
            recurvedTrigger.JobGroupName = "TestJobGroup";

            recurvedTrigger.Frequency = new Daily() { RepeatedDays = 1 };

            start = new DateTime(1900, 1, 1, 1, 0, 0);
            end = new DateTime(1900, 1, 1, 14, 0, 0);
            recurvedTrigger.DailyFrequency = new RepeatedDailyFrequency() { StartingAtUTC = start, EndingAtUTC = end, RepeatedMinutes = 1 };

            recurvedTrigger.StartDateUTC = new DateTime(2008, 1, 1).ToUniversalTime();
            _scheduleService.ScheduleJob(job, recurvedTrigger);

            Thread.Sleep(90 * 1000);
        }

        [Fact]
        public void RecurvedTriggerWithRepeatedDailyFrequency()
        {
            _scheduleService.ClearJobsAndTriggers();
            _scheduleService.ClearJobHistory();

            Job job = new Job();
            job.Name = "RecurvedTriggerWithRepeatedDailyFrequencyJob";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";

            RecurvedTrigger recurvedTrigger = new RecurvedTrigger();
            recurvedTrigger.Name = "RecurvedTriggerWithRepeatedDailyFrequencyTrigger";
            recurvedTrigger.Group = "TestTriggerGroup";
            recurvedTrigger.JobName = "RecurvedTriggerWithRepeatedDailyFrequencyJob";
            recurvedTrigger.JobGroupName = "TestJobGroup";

            recurvedTrigger.Frequency = new Daily() { RepeatedDays = 1};

            DateTime start = new DateTime(1900, 1, 1, 1, 0, 0);
            DateTime end = new DateTime(1900, 1, 1, 14, 0, 0);
            recurvedTrigger.DailyFrequency = new RepeatedDailyFrequency() {StartingAtUTC = start, EndingAtUTC = end, RepeatedMinutes = 1};
            
            recurvedTrigger.StartDateUTC = new DateTime(2008,1,1).ToUniversalTime();

            DateTime firstFireTime = _scheduleService.ScheduleJob(job, recurvedTrigger);
            Console.WriteLine(firstFireTime);

            JobState state = _scheduleService.GetStateByJob("RecurvedTriggerWithRepeatedDailyFrequencyJob", "TestJobGroup");
            Assert.True(state.State == State.Waiting);

            //update trigger
            var triggers = _scheduleService.GetTriggersByJob("RecurvedTriggerWithRepeatedDailyFrequencyJob",
                                                             "TestJobGroup");

            Assert.True(triggers.Length == 1);

            recurvedTrigger = triggers[0] as RecurvedTrigger;
            Assert.NotNull(recurvedTrigger);

            recurvedTrigger.Frequency = new Daily() { RepeatedDays = 1 };
            start = new DateTime(1900, 1, 1, 1, 0, 0);
            end = new DateTime(1900, 1, 1, 14, 0, 0);

            recurvedTrigger.DailyFrequency = new RepeatedDailyFrequency() { StartingAtUTC = start, EndingAtUTC = end, RepeatedMinutes = 1 };
            recurvedTrigger.StartDateUTC = new DateTime(2008, 2, 1).ToUniversalTime();

            _scheduleService.CreateTrigger(recurvedTrigger);

            triggers = _scheduleService.GetTriggersByJob("RecurvedTriggerWithRepeatedDailyFrequencyJob",
                                                             "TestJobGroup");


            // wait 180 seconds to show jobs
            Thread.Sleep(180 * 1000);
        }

        [Fact]
        public void RecurvedTriggerWithOnceDailyFrequencyTest()
        {
            Job job = new Job();
            job.Name = "RecurvedTriggerWithOnceDailyFrequencyJob";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";

            RecurvedTrigger recurvedTrigger = new RecurvedTrigger();
            recurvedTrigger.Name = "RecurvedTriggerWithOnceDailyFrequencyTrigger";
            recurvedTrigger.Group = "TestTriggerGroup";
            recurvedTrigger.JobName = "RecurvedTriggerWithOnceDailyFrequencyFrequencyJob";
            recurvedTrigger.JobGroupName = "TestJobGroup";

            recurvedTrigger.Frequency = new Daily() { RepeatedDays = 1 };

            recurvedTrigger.DailyFrequency = new OnceDailyFrequency() { OccurTime = new TimeSpan(10, 35, 00) };

            DateTime firstFireTime = _scheduleService.ScheduleJob(job, recurvedTrigger);
            Console.WriteLine(firstFireTime);

            JobState state = _scheduleService.GetStateByJob("RecurvedTriggerWithOnceDailyFrequencyJob", "TestJobGroup");
            Assert.True(state.State == State.Waiting);

            _scheduleService.DisableJob("RecurvedTriggerWithOnceDailyFrequencyJob", "TestJobGroup");
            state = _scheduleService.GetStateByJob("RecurvedTriggerWithOnceDailyFrequencyJob", "TestJobGroup");
            Assert.True(state.State == State.Disabled);

            _scheduleService.EnableJob("RecurvedTriggerWithOnceDailyFrequencyJob", "TestJobGroup");
            state = _scheduleService.GetStateByJob("RecurvedTriggerWithOnceDailyFrequencyJob", "TestJobGroup");
            Assert.True(state.State == State.Waiting);

            // wait 120 seconds to show jobs
            Thread.Sleep(120 * 1000);
        
        }

        [Fact]
        public void RunJobOnceNow()
        {
            Job job = new Job();
            job.Name = "RunJobOnceNowJob";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";
            job.RequestsRecovery = true;

            _scheduleService.CreateJob(job);
            _scheduleService.RunJobOnceNow("RunJobOnceNowJob", "TestJobGroup");

            Thread.Sleep(10 * 1000);

            JobHistory[] history = _scheduleService.GetHistoryByJob("RunJobOnceNowJob", "TestJobGroup");
            JobState state = _scheduleService.GetStateByJob("RunJobOnceNowJob", "TestJobGroup");

            Thread.Sleep(10 * 1000);
            state = _scheduleService.GetStateByJob("RunJobOnceNowJob", "TestJobGroup");

        }

        [Fact]
        public void StopRunningJob()
        {
            Job job = new Job();
            job.Name = "StopRunningJob";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.LongJobWork";

            _scheduleService.CreateJob(job);
            _scheduleService.RunJobOnceNow("StopRunningJob", "TestJobGroup");

            Thread.Sleep(20 * 1000);

            JobState state = _scheduleService.GetStateByJob("StopRunningJob", "TestJobGroup");
            Assert.True(state.State == State.Running);

            _scheduleService.StopRunningJob("StopRunningJob", "TestJobGroup");

            state = _scheduleService.GetStateByJob("StopRunningJob", "TestJobGroup");
            Assert.True(state.State == State.Disabled);
        }

        [Fact]
        public void UpdateJobAndTrigger()
        {
            _scheduleService.ClearJobsAndTriggers();
            _scheduleService.ClearJobHistory();

            Job job = new Job();
            job.Name = "UpdateJob";
            job.Group = "TestJobGroup";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";

            RecurvedTrigger recurvedTrigger = new RecurvedTrigger();
            recurvedTrigger.Name = "UpdateTrigger";
            recurvedTrigger.Group = "TestTriggerGroup";
            recurvedTrigger.JobName = "UpdateJob";
            recurvedTrigger.JobGroupName = "TestJobGroup";
            recurvedTrigger.Frequency = new Daily() { RepeatedDays = 1 };
            recurvedTrigger.DailyFrequency = new OnceDailyFrequency() { OccurTime = new TimeSpan(10, 35, 00) };

            _scheduleService.CreateJob(job);
            _scheduleService.CreateTrigger(recurvedTrigger);

            //update
            job.Name = "UpdateJob1";
            _scheduleService.UpdateJob("UpdateJob", "TestJobGroup", job);

            recurvedTrigger.JobName = job.Name;
            recurvedTrigger.JobGroupName = job.Group;
            recurvedTrigger.Frequency = new Daily() { RepeatedDays = 2 };
            recurvedTrigger.DailyFrequency = new OnceDailyFrequency() { OccurTime = new TimeSpan(12, 30, 00) };
            _scheduleService.UpdateTrigger("UpdateTrigger", "TestTriggerGroup", recurvedTrigger);

            //validation
            Job newJob = _scheduleService.GetJob("UpdateJob1", "TestJobGroup");
            Assert.NotNull(newJob);
            Assert.Null(_scheduleService.GetJob("UpdateJob", "TestJobGroup"));

            Services.JobSchelude.Trigger myTrigger = _scheduleService.GetTrigger("UpdateTrigger", "TestTriggerGroup");
            Assert.NotNull(myTrigger);

            //update again
            job.Name = "UpdateJob2";

            recurvedTrigger.JobName = job.Name;
            recurvedTrigger.JobGroupName = job.Group;
            recurvedTrigger.Frequency = new Daily() { RepeatedDays = 3 };
            recurvedTrigger.DailyFrequency = new OnceDailyFrequency() { OccurTime = new TimeSpan(12, 40, 00) };

            _scheduleService.UpdateJobAndTrigger("UpdateJob1", "TestJobGroup", job, "UpdateTrigger", "TestTriggerGroup", recurvedTrigger);
            
            //validation
            newJob = _scheduleService.GetJob("UpdateJob2", "TestJobGroup");
            Assert.NotNull(newJob);
            Assert.Null(_scheduleService.GetJob("UpdateJob1", "TestJobGroup"));

            myTrigger = _scheduleService.GetTrigger("UpdateTrigger", "TestTriggerGroup");
            Assert.NotNull(myTrigger);
        }

        [Fact]
        public void RemoteTest()
        {
            ScheduleServiceClient proxy = new ScheduleServiceClient();
            JasonSoft.Services.JobSchedule.ClientProxy.JobState[] jobStats = proxy.GetStatesByJobGroup("KM");

            Job job = new Job();
            job.Name = "RemoteTestJob1";
            job.Group = "KM";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";
            _scheduleService.CreateJob(job);

            job = new Job();
            job.Name = "RemoteTestJob2";
            job.Group = "KM";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";
            _scheduleService.CreateJob(job);

            job = new Job();
            job.Name = "RemoteTestJob3";
            job.Group = "KM";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";
            _scheduleService.CreateJob(job);

            job = new Job();
            job.Name = "RemoteTestJob4";
            job.Group = "KM";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";
            _scheduleService.CreateJob(job);

            job = new Job();
            job.Name = "RemoteTestJob5";
            job.Group = "KM";
            job.AssemblyName = "JasonSoft.Core.Test.dll";
            job.ClassName = "JasonSoft.Tests.Schelude.JobWork";
            _scheduleService.CreateJob(job);

            proxy.Close();
        }
    }
}
