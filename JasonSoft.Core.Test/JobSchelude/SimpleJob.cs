using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JasonSoft.Components.Logging;
using JasonSoft.Components.Quartz;

namespace JasonSoft.Tests.JobSchelude
{
    public class SimpleJob : IStatefulJob 
    {
        private static ILog _log = LogManager.GetLogger(typeof(SimpleJob));

        public void Execute(JobExecutionContext context)
        {
            _log.Info(string.Format("SimpleJob! - {0}", System.DateTime.Now.ToString("r")));
        }
    }
}
