using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Transactions;
using JasonSoft.Components.Quartz.Impl.Calendar;
using JasonSoft;
using JasonSoft.Components.Quartz;
using JasonSoft.Components.Quartz.Impl;
using JasonSoft.Services.JobSchelude.Data;
using JasonSoft.Data;

namespace JasonSoft.Services.JobSchelude
{
    public class ScheduleService : IScheduleService
    {
        public ScheduleService()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            _sched = sf.GetScheduler();
            
            

            if (_sched.IsStarted == false) _sched.Start();

            //add listener
            bool answer = true;
            if (!_sched.GlobalJobListeners.IsNullOrEmpty())
            {
                foreach (var listener in _sched.GlobalJobListeners)
                {
                    JobHistoryListener jobListener = listener as JobHistoryListener;
                    if (jobListener != null && jobListener.Name == "JobHistoryListener") answer = false;
                }
            }
            if (answer) _sched.AddGlobalJobListener(new JobHistoryListener());
        }


        IScheduler _sched = null;


        public DateTime ScheduleJob(Job job, Trigger trigger)
        {
            if(job.Name.IsNullOrEmpty() || job.Group.IsNullOrEmpty()) throw new ArgumentNullException("job","Job name or GroupName can't be null or empty");
            trigger.JobName = job.Name;
            trigger.JobGroupName = job.Group;

            JobDetail quartzJobDetail = new JobAdapter(job).QuartzJobDetail;
            TriggerAdapter triggerAdapter = new TriggerAdapter(trigger);
            JasonSoft.Components.Quartz.Trigger quartzTrigger = triggerAdapter.QuartzTrigger;
            AddOrUpdateCalendar(triggerAdapter);
            
            DateTime firstFireTime = _sched.ScheduleJob(quartzJobDetail, quartzTrigger);
            return firstFireTime;
        }

        public Job GetJob(string jobName, string jobGroupName)
        {
            JobDetail quartzJobDetail = _sched.GetJobDetail(jobName, jobGroupName);
            if(quartzJobDetail == null)
                return null;
            else
                return new JobAdapter(quartzJobDetail).Job;
        }

        public Job[] GetJobsByGroup(string jobGroupName)
        {
            string[] jobNames = _sched.GetJobNames(jobGroupName);

            List<Job> results = null;

            if (!jobNames.IsNullOrEmpty())
            {
                results = new List<Job>();

                foreach (string jobName in jobNames)
                {
                    Job job = GetJob(jobName, jobGroupName);
                    if (job != null) results.Add(job);                    
                }
            }

            if (results == null)
                return null;
            else
                return results.ToArray();
        }

        public void CreateJob(Job job)
        {
            JobDetail quartzJobDetail = new JobAdapter(job).QuartzJobDetail;
            _sched.AddJob(quartzJobDetail, true);
        }

        public void DeleteJob(string jobName, string jobGroupName)
        {
            _sched.DeleteJob(jobName, jobGroupName);     
        }

        public void DeleteJobsByGroup(string jobGroupName)
        {

            Job[] jobs = GetJobsByGroup(jobGroupName);

            if (!jobs.IsNullOrEmpty()) 
            {
                foreach (Job job in jobs) DeleteJob(job.Name, job.Group);            
            }            
        }

        public void UpdateJob(String oldJobName, String oldJobGroupName, Job newJob)
        {
            //ensure the job exists
            
            Job oldJob = GetJob(oldJobName, oldJobGroupName);

            if (oldJob == null)
                throw new InvalidOperationException(String.Format("The jobname is called {0} in the {1} jobgroup doesn't exist.", oldJobName, oldJobGroupName));


            if (oldJobName != newJob.Name || oldJobGroupName != newJob.Group)
            {
                if (GetJob(newJob.Name, newJob.Group) != null)
                    throw new InvalidOperationException(String.Format("The jobname is called {0} in the {1} jobgroup has already existed.", newJob.Name, newJob.Group));

                Trigger[] triggers = GetTriggersByJob(oldJobName, oldJobGroupName);

                using (TransactionScope scope = new TransactionScope())
                {
                    DisableJob(oldJobName, oldJobGroupName);

                    //update triggers jobname and jobGroupName
                    if (!triggers.IsNullOrEmpty())
                    {
                        foreach (var trigger in triggers)
                        {
                            trigger.JobName = newJob.Name;
                            trigger.JobGroupName = newJob.Group;
                        }
                    }

                    //delete old job and triggers
                    DeleteJob(oldJobName, oldJobGroupName);

                    //create new job and trigger
                    CreateJob(newJob);
                    if (!triggers.IsNullOrEmpty())
                    {
                        foreach (var trigger in triggers)
                        {
                            CreateTrigger(trigger);
                        }
                    }

                    scope.Complete();
                }
            }
            else
            {
                JobDetail quartzJobDetail = new JobAdapter(newJob).QuartzJobDetail;
                _sched.AddJob(quartzJobDetail, true);
            }
        }

        public Trigger GetTrigger(string triggerName, string triggerGroupName)
        {
            JasonSoft.Components.Quartz.Trigger quartzTrigger = _sched.GetTrigger(triggerName, triggerGroupName);
            String calendarName = triggerGroupName + "." + triggerName;
            ICalendar quartzDailyCalendar = _sched.GetCalendar(calendarName);
            return new TriggerAdapter(quartzTrigger, quartzDailyCalendar).Trigger;
        }
        
        public Trigger[] GetTriggersByJob(string jobName, string jobGroupName)
        {
            if (jobName.IsNullOrEmpty() || jobGroupName.IsNullOrEmpty()) throw new ArgumentNullException("jobName or jobGroupName can't be null or empty");

            JasonSoft.Components.Quartz.Trigger[] quartzTriggers = _sched.GetTriggersOfJob(jobName, jobGroupName);
            List<Trigger> results = null;

            if (!quartzTriggers.IsNullOrEmpty())
            {
                results = new List<Trigger>();

                foreach (var quartzTrigger in quartzTriggers)
                {
                    String calendarName = quartzTrigger.Group + "." + quartzTrigger.Name;
                    ICalendar quartzDailyCalendar = _sched.GetCalendar(calendarName);
                    Trigger result = new TriggerAdapter(quartzTrigger, quartzDailyCalendar).Trigger;
                    results.Add(result);
                }
            }

            if(results == null)
                return null;
            else 
                return results.ToArray();
        }

        public void CreateTrigger(Trigger trigger)
        {
            if(trigger.JobName.IsNullOrEmpty() || trigger.JobGroupName.IsNullOrEmpty())
                throw new ArgumentException("TriggerName or TriggerGroupName can't be null or empty");

            //ensure job exists
            Job job = GetJob(trigger.JobName, trigger.JobGroupName);
            if (job == null) throw new NullReferenceException("job doesn't exist");

            TriggerAdapter triggerAdapter = new TriggerAdapter(trigger);
            JasonSoft.Components.Quartz.Trigger quartzTrigger = triggerAdapter.QuartzTrigger;
            AddOrUpdateCalendar(triggerAdapter);

            _sched.ScheduleJob(quartzTrigger);
        }

        public void DeleteTrigger(string triggerName, string triggerGroupName)
        {
            _sched.UnscheduleJob(triggerName, triggerGroupName);

            string calendarName = triggerGroupName + "." + triggerName;
            _sched.DeleteCalendar(calendarName);
        }

        
        public void UpdateTrigger(String oldTriggerName, String oldTriggerGroupName, Trigger newTrigger)
        {
            if (newTrigger.JobName.IsNullOrEmpty() || newTrigger.JobGroupName.IsNullOrEmpty())
                throw new ArgumentException("TriggerName or triggerGroupName can't be null or empty");
            
            //ensure trigger exists.
            Trigger oldTrigger = GetTrigger(oldTriggerName, oldTriggerGroupName);

            if(oldTrigger == null)
                throw new NullReferenceException(String.Format("Can't find the trigger is called {0} in the {1} trigger group.", oldTriggerName, oldTriggerGroupName));

            if (GetTrigger(newTrigger.JobName, newTrigger.JobGroupName) != null)
                throw new InvalidOperationException(String.Format("The trigger is called {0} has already existed in the {1} trigger group", newTrigger.Name, newTrigger.Group));

            if(GetJob(newTrigger.JobName, newTrigger.JobGroupName) == null)
                throw new NullReferenceException(String.Format("Can't find the job is called {0} in the {1} jobgroup.", newTrigger.JobName, newTrigger.JobGroupName));

            using (TransactionScope scope = new TransactionScope())
            {
                if (oldTriggerName != newTrigger.Name || oldTriggerGroupName != newTrigger.Group)
                {
                    DeleteTrigger(oldTriggerName, oldTriggerGroupName);
                    CreateTrigger(newTrigger);
                }
                else
                {
                    TriggerAdapter triggerAdapter = new TriggerAdapter(newTrigger);
                    JasonSoft.Components.Quartz.Trigger newQuartzTrigger = triggerAdapter.QuartzTrigger;
                    AddOrUpdateCalendar(triggerAdapter);

                    _sched.RescheduleJob(newTrigger.Name, newTrigger.Group, newQuartzTrigger);
                }

                scope.Complete();
            }
        }

        public void UpdateJobAndTrigger(string oldJobName, string oldJobGroupName, Job newJob, string oldTriggerName,
                                        string oldTriggerGroupName, Trigger newTrigger)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                UpdateJob(oldJobName, oldJobGroupName, newJob);
                UpdateTrigger(oldTriggerName, oldTriggerGroupName, newTrigger);
                scope.Complete();
            }
        }

        public void RunJobOnceNow(string jobName, string jobGroupName)
        {
            if(!IsJobRunning(jobName, jobGroupName))
                _sched.TriggerJob(jobName, jobGroupName);
                //_sched.TriggerJobWithVolatileTrigger(jobName, jobGroupName);
        }

        public void EnableJob(string jobName, string jobGroupName)
        {
            _sched.ResumeJob(jobName, jobGroupName);
        }

        public void DisableJob(string jobName, string jobGroupName)
        {
            StopRunningJob(jobName, jobGroupName);
            _sched.PauseJob(jobName, jobGroupName); 
        }

        public void StopRunningJob(string jobName, string jobGroupName)
        {
            if(IsJobRunning(jobName, jobGroupName))
            {
                _sched.Interrupt(jobName, jobGroupName);
                
                //quartz is starting a new thread to stop the job.  Therefore, currentthread should be waiting until the job is stopped.
                //ensure job is stopped.
                int retryCount = 0;

                while(retryCount < 6)
                {
                    if(IsJobRunning(jobName, jobGroupName) == false) return;

                    Thread.Sleep(5 * 1000);
                    retryCount++;
                }

                throw new InvalidOperationException("Job name is {0} in {1} group can't be stopped");
            }
        }

        public JobHistory[] GetHistoryByJob(string jobName, string jobGroupName)
        {
            ServiceDataContext serviceDB = new ServiceDataContext();

            return (from h in serviceDB.GetTable<JobHistory>()
                           where h.JobName == jobName && h.JobGroupName == jobGroupName && (h.State == State.Success || h.State == State.Failed || h.State == State.StopManually)
                           select h).ToArray();            
        }

        public JobState GetStateByJob(string jobName, string jobGroupName)
        {
            //ensure job exist
            JobDetail jobdetail = _sched.GetJobDetail(jobName, jobGroupName);
            
            if(jobdetail == null) return null;

            JobState result = new JobState(){ JobName = jobName, JobGroupName = jobGroupName };

            //get job history
            ServiceDataContext serviceDB = new ServiceDataContext();

            JobHistory history = (from h in serviceDB.GetTable<JobHistory>()
                                  where h.JobName == jobName && h.JobGroupName == jobGroupName && (h.State == State.Success || h.State == State.Failed || h.State == State.StopManually)
                                  select h).FirstOrDefault();

            if (history != null)
            {
                result.LastFireState = history.State;
                result.PreviousFireTimeUTC = history.BeginDateUTC;
            }
            else
            {
                result.LastFireState = State.Nothing;
            }

            //get job next fire time
            JasonSoft.Components.Quartz.Trigger[] quartzTriggers = _sched.GetTriggersOfJob(jobName, jobGroupName);
            
            if(!quartzTriggers.IsNullOrEmpty())
            {
                foreach (var trigger in quartzTriggers) 
                {
                    DateTime? nextFireTime = trigger.GetNextFireTimeUtc();

                    if (nextFireTime != null) 
                    {
                        if (result.NextFireTimeUTC == null)
                        {
                            result.NextFireTimeUTC = nextFireTime;                            
                        }
                        else 
                        {
                            if (nextFireTime > result.NextFireTimeUTC) result.NextFireTimeUTC = nextFireTime;
                        }
                    }

                    if (_sched.GetTriggerState(trigger.Name, trigger.Group) == TriggerState.Normal) result.State = State.Waiting;
                }
            }

            if(IsJobRunning(jobName, jobGroupName)) result.State = State.Running;

            //set progress
            String progressKey = jobGroupName + "." + jobName + ".Progress";
            if (_sched.Context[progressKey] != null) result.Progress = _sched.Context[progressKey].ToString();

            return result;
        }

        public JobState[] GetStatesByJobGroup(string jobGroupName)
        {
            String[] jobNames = _sched.GetJobNames(jobGroupName);

            if(!jobNames.IsNullOrEmpty())
            {
                List<JobState> results = new List<JobState>();

                foreach (var jobName in jobNames)
                {
                    results.Add(GetStateByJob(jobName, jobGroupName));
                }

                return results.ToArray();
            }

            return null;
        }

        private Boolean IsJobRunning(String jobName, String jobGroupName)
        {
            IList<JobExecutionContext> jobExecutionContexts = _sched.GetCurrentlyExecutingJobs();
            
            if(!jobExecutionContexts.IsNullOrEmpty())
            {
                foreach(JobExecutionContext jobContext in jobExecutionContexts)
                {
                    if(jobContext.JobDetail.Name == jobName && jobContext.JobDetail.Group == jobGroupName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void AddOrUpdateCalendar(TriggerAdapter triggerAdapter)
        {
            String calendarName = triggerAdapter.Trigger.Group + "." + triggerAdapter.Trigger.Name;
            if(triggerAdapter.QuartzCalendar != null)
            {
                _sched.AddCalendar(calendarName, triggerAdapter.QuartzCalendar, true, true);
            }
        }


        public void ClearJobHistory()
        {
            using(ServiceDataContext serviceDB = new ServiceDataContext())
            {
                serviceDB.GetTable<JobHistory>().Truncate();
            }
        }

        public void ClearJobsAndTriggers()
        {
            // unschedule jobs
            string[] groups = _sched.TriggerGroupNames;
            for (int i = 0; i < groups.Length; i++)
            {
                string[] names = _sched.GetTriggerNames(groups[i]);

                for (int j = 0; j < names.Length; j++)
                {
                    _sched.UnscheduleJob(names[j], groups[i]);
                }
            }


            // delete jobs
            groups = _sched.JobGroupNames;
            for (int i = 0; i < groups.Length; i++)
            {
                string[] names = _sched.GetJobNames(groups[i]);

                for (int j = 0; j < names.Length; j++)
                {
                    _sched.DeleteJob(names[j], groups[i]);
                }
            }
        }

    }
}
