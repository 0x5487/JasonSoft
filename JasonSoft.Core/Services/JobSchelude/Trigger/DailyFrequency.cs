using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public abstract class DailyFrequency
    {
        public abstract string CronExpression { get; }
    }
}
