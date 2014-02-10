using System.Runtime.Serialization;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public class Daily : Frequency
    {

        private int _days = 1;

        [DataMember]
        public int RepeatedDays
        {
            get { return _days; }
            set
            {
                _days = value;

                if (value <= 0)
                {
                    _days = 1;
                }
            }
        }


        public override string CronExpression
        {
            get
            {
                return string.Format("{0} {1} {2}", "1/" + _days.ToString(), "*", "?");
            }
        }
    }
}