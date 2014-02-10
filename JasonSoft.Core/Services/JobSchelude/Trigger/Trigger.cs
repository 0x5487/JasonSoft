using System;
using System.Runtime.Serialization;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public class Trigger
    {
        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Group { get; set; }

        [DataMember]
        public String JobName { get; set; }

        [DataMember]
        public String JobGroupName { get; set; }
       
    }
}