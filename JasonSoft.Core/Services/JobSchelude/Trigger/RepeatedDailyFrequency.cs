using System;
using System.Runtime.Serialization;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public class RepeatedDailyFrequency : DailyFrequency
    {
        [DataMember]
        public DateTime StartingAtUTC { get; set; }

        [DataMember]
        public DateTime EndingAtUTC { get; set; }

        private int _minute = 1;

        [DataMember]
        public int RepeatedMinutes
        {
            get { return _minute; }
            set { _minute = value; }
        }

        public override string CronExpression
        {
            get
            {
                return string.Format("{0} {1} {2}", 0, "0/" + _minute, "*");
            }
        }

    }
}