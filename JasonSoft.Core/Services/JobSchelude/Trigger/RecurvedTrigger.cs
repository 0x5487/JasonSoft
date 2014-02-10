using System;
using System.Runtime.Serialization;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public class RecurvedTrigger : Trigger
    {
        public RecurvedTrigger()
        {
            this.Frequency = new Daily();
            this.DailyFrequency = new OnceDailyFrequency();

            this.StartDateUTC = DateTime.UtcNow;
        }

        [DataMember]
        public Frequency Frequency { get; set; }

        [DataMember]
        public DailyFrequency DailyFrequency { get; set; }

        [DataMember]
        public DateTime StartDateUTC { get; set; }

        [DataMember]
        public DateTime? EndDateUTC { get; set; }

        public String GetCronExpression()
        {
            string date = DailyFrequency.CronExpression;
            string month = Frequency.CronExpression;
            return date + " " + month;
        }
    }
    
}