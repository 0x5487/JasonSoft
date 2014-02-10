using System;
using System.Runtime.Serialization;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public  class JobState
    {
        public JobState()
        {
            State = State.Disabled;
            Progress = String.Empty;
        }

        [DataMember]
        public String JobName { get; internal set; }

        [DataMember]
        public String JobGroupName { get; internal set; }

        [DataMember]
        public State State { get; internal set; }

        [DataMember]
        public State LastFireState { get; internal set; }

        [DataMember]
        public DateTime? PreviousFireTimeUTC { get; internal set; }

        [DataMember]
        public DateTime? NextFireTimeUTC { get; internal set; }

        [DataMember]
        public String Progress { get; internal set; }

    }
}