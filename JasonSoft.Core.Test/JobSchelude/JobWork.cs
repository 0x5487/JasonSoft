using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JasonSoft.Services.JobSchelude;
using JasonSoft.Components.Logging;
using System.Threading;

namespace JasonSoft.Tests.Schelude 
{
    public class JobWork : IJobWork
    {
        private static ILog _log = LogManager.GetLogger(typeof(JobWork));

        public void Execute(JobRunTimeInfo jobRunTimeInfo)
        {
            for(Int32 i=0; i <7; i++)
            {
                _log.Info(string.Format("Hello World"));
                jobRunTimeInfo.Progress = i.ToString();
                Thread.Sleep(10 * 1000);
            }
            
            
        }
    }
}
