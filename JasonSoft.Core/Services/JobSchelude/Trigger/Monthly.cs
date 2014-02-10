using System.Runtime.Serialization;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public class Monthly : Frequency
    {

        private int _dayOfMonth = 1;

        [DataMember]
        public int DayOfMonth
        {
            get { return _dayOfMonth; }
            set
            {
                _dayOfMonth = value;

                if (value <= 0)
                {
                    _dayOfMonth = 1;
                }
                else if (value > 31)
                {
                    _dayOfMonth = 31;
                }
            }

        }

        private int _months = 1;

        [DataMember]
        public int RepeatedMonths
        {
            get { return _months; }
            set
            {
                _months = value;

                if (value <= 0)
                {
                    _dayOfMonth = 1;
                }
            }
        }


        public override string CronExpression
        {
            get
            {
                return string.Format("{0} {1} {2}", _dayOfMonth.ToString(), "1/" + _months, "?");
            }
        }
    }
}