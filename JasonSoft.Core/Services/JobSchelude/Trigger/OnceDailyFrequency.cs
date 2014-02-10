using System;
using System.Runtime.Serialization;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public class OnceDailyFrequency : DailyFrequency
    {
        private TimeSpan _occurTime;

        [DataMember]
        public TimeSpan OccurTime
        {
            get { return _occurTime; }
            set { _occurTime = value; }
        }

        public override string CronExpression
        {
            get
            {
                if (_occurTime != null)
                {
                    return string.Format("{0} {1} {2}", _occurTime.Seconds, _occurTime.Minutes, _occurTime.Hours);
                }

                return null;
            }
        }
    }
}