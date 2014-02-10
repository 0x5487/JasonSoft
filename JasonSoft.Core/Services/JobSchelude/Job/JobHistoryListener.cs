using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using JasonSoft.Components.Quartz;
using JasonSoft.Services.JobSchelude.Data;
using JasonSoft;

namespace JasonSoft.Services.JobSchelude
{
    internal class JobHistoryListener : IJobListener
    {
        public string Name
        {
            get { return "JobHistoryListener"; }
        }

        public void JobToBeExecuted(JobExecutionContext context)
        {
            using (ServiceDataContext serviceDB = new ServiceDataContext())
            {
                JobHistory history = new JobHistory();
                history.InstanceName = context.Scheduler.SchedulerInstanceId;
                history.JobName = context.JobDetail.Name;
                history.JobGroupName = context.JobDetail.Group;
                history.BeginDateUTC = DateTime.UtcNow;

                if (context.Trigger.Group == "MANUAL_TRIGGER") history.Mode = Mode.Manual;

                history.State = State.Running;

                serviceDB.GetTable<JobHistory>().InsertOnSubmit(history);
                serviceDB.SubmitChanges();

                context.JobDetail.JobDataMap["HistoryID"] = history.ID.ToString();
            }
        }

        public void JobExecutionVetoed(JobExecutionContext context)
        {
            using (ServiceDataContext serviceDB = new ServiceDataContext())
            {
                Int32 historyID = context.JobDetail.JobDataMap["HistoryID"].ChangeTypeTo<int>();
                JobHistory history = (from h in serviceDB.GetTable<JobHistory>()
                                      where h.ID == historyID
                                      select h).SingleOrDefault();

                if (history != null)
                {
                    history.State = State.StopManually;
                    history.Message = "StopManually";
                    history.EndDateUTC = DateTime.UtcNow;
                    history.Period = (int) history.EndDateUTC.Value.Subtract(history.BeginDateUTC).TotalSeconds;
                    serviceDB.SubmitChanges();

                    context.JobDetail.JobDataMap["HistoryID"] = 0;
                }
            }
        }

        public void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException)
        {
            Int32 historyID = context.JobDetail.JobDataMap["HistoryID"].ChangeTypeTo<int>();
            if(historyID == 0) return;

            using (ServiceDataContext serviceDB = new ServiceDataContext())
            {
                JobHistory history = (from h in serviceDB.GetTable<JobHistory>()
                                      where h.ID == historyID
                                      select h).SingleOrDefault();

                if (history != null)
                {
                    if (jobException == null)
                    {
                        history.State = State.Success;
                        history.Message = "Success";
                    }
                    else
                    {
                        history.State = State.Failed;
                        history.Message = jobException.InnerException.InnerException.Message +
                                          jobException.InnerException.InnerException.StackTrace;
                    }

                    history.EndDateUTC = DateTime.UtcNow;
                    history.Period = (int) history.EndDateUTC.Value.Subtract(history.BeginDateUTC).TotalSeconds;
                    serviceDB.SubmitChanges();
                }
            }
        }
    }
}
