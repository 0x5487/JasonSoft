using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using JasonSoft.Components.Logging;
using JasonSoft.Components.Quartz;
using JasonSoft.Reflection;


namespace JasonSoft.Services.JobSchelude
{
    internal class JobExecuter : IInterruptableJob, IStatefulJob
    {
        private static ILog _log = LogManager.GetLogger(typeof(JobExecuter));

        Thread runningThread;

        public void Execute(JobExecutionContext context)
        {
            runningThread = Thread.CurrentThread;
 
            try
            {
                String assemblyName = context.JobDetail.JobDataMap["assembly_name"].ToString();
                Assembly asm = Assembly.LoadFrom(assemblyName);
                String className = context.JobDetail.JobDataMap["class_name"].ToString();
                Type targetType = asm.GetType(className, true, false);

                object targetObject = targetType.CreaetInstance();
                IJobWork targetJob = (IJobWork)targetObject;

                Dictionary<string, object> argument = (Dictionary<string, object>)context.JobDetail.JobDataMap["argument"];

                Job job = new Job()
                              {
                                  Name = context.JobDetail.Name,
                                  Group = context.JobDetail.Group,
                                  Description = context.JobDetail.Description,
                                  Argument = argument,
                                  AssemblyName = assemblyName,
                                  ClassName = className,
                                  RequestsRecovery = context.JobDetail.RequestsRecovery
                              };

                JobRunTimeInfo jobRunTimeInfo = new JobRunTimeInfo(context.Scheduler, job)
                                                  {
                                                      FireTime = context.FireTimeUtc.Value,
                                                      NextFireTime = context.NextFireTimeUtc,
                                                      Recovering = context.Recovering
                                                  };

                targetJob.Execute(jobRunTimeInfo);
                jobRunTimeInfo.Progress = String.Empty;

            }
            catch (ThreadAbortException)
            {
                new JobHistoryListener().JobExecutionVetoed(context);
                Thread.ResetAbort();
                return;
            }
        }

        public void Interrupt()
        {
            if (runningThread != null && runningThread.IsAlive)
            {
                runningThread.Abort();
            }
        }
    }
}