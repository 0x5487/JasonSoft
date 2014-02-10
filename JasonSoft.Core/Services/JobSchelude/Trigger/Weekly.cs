using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public class Weekly: Frequency
    {
        public Weekly()
        {
            RepeatedWeeks = 1;
        }

        [DataMember]
        public int RepeatedWeeks { get; set; }

        [DataMember]
        public DayOfWeek DayOfWeek { get; set; }

        

        public override string CronExpression
        {
            get
            {
                return string.Format("{0} {1} {2}", "?", "*", ((int)DayOfWeek) + 1 + "/" + RepeatedWeeks * 8);
            }
        }
    }
}
