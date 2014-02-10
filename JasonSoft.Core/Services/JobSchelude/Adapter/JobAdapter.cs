using System;
using System.Collections.Generic;
using JasonSoft.Components.Quartz;


namespace JasonSoft.Services.JobSchelude
{
    internal class JobAdapter
    {
        public JobAdapter(JobDetail quartzJobDetail)
        {
            Job = new Job
                      {
                          Name = quartzJobDetail.Name,
                          Group = quartzJobDetail.Group,
                          Description = quartzJobDetail.Description,
                          RequestsRecovery = quartzJobDetail.RequestsRecovery,
                          AssemblyName = quartzJobDetail.JobDataMap["assembly_name"].ToString(),
                          ClassName = quartzJobDetail.JobDataMap["class_name"].ToString(),
                          Argument = ((Dictionary<string, object>) quartzJobDetail.JobDataMap["argument"])
                      };
            
            this.QuartzJobDetail = quartzJobDetail;
        }

        public JobAdapter(Job job)
        {
            QuartzJobDetail = new JobDetail(job.Name, job.Group, typeof(JobExecuter), false, true, job.RequestsRecovery);
            QuartzJobDetail.Description = job.Description;
            QuartzJobDetail.JobDataMap["assembly_name"] = job.AssemblyName;
            QuartzJobDetail.JobDataMap["class_name"] = job.ClassName;
            QuartzJobDetail.JobDataMap["argument"] = job.Argument;
            
            this.Job = job;
        }

        public JobDetail QuartzJobDetail { get; set; }
        public Job Job { get; set; }
    }
}