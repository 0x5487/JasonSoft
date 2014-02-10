using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JasonSoft.Components.Quartz;

namespace JasonSoft.Services.JobSchelude
{
    [Serializable]
    public class JobRunTimeInfo
    {
        private IScheduler _scheduler;
        private String _progressKey;

        internal JobRunTimeInfo(IScheduler scheduler, Job job)
        {
            _scheduler = scheduler;
            Job = job;

            _progressKey = job.Group + "." + job.Name + ".Progress";
            Progress = String.Empty;
        }
 
        public Job Job { get; internal set;}

        public Boolean Recovering { get; internal set;}
 
        public DateTime FireTime { get; internal set; }
  
        public DateTime? NextFireTime { get; internal set; }
 
        public String Progress
        {
            get
            {
                if (_scheduler.Context[_progressKey] == null)
                    return String.Empty;
                else
                    return _scheduler.Context[_progressKey].ToString();
            }

            set { _scheduler.Context.Put(_progressKey, value);}
        }

    }
}