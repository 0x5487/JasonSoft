using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using JasonSoft.Components.Logging;
using JasonSoft.Services.JobSchelude;

namespace JasonSoft.Tests.Schelude
{
    class LongJobWork : IJobWork
    {
        private static ILog _log = LogManager.GetLogger(typeof(LongJobWork));

        public void Execute(JobRunTimeInfo jobRunTimeInfo)
        {
            _log.Info(string.Format("Long Job! - {0}", System.DateTime.Now.ToString("r")));
            Thread.Sleep(60 * 1000);
        }
    }
}
