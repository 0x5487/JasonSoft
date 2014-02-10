using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public class Job
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Group { get; set; }

        [DataMember]
        public String Description { get; set;}

        [DataMember]
        public Boolean RequestsRecovery { get; set;}

        [DataMember]
        public string AssemblyName { get; set; }

        [DataMember]
        public string ClassName { get; set; }

        [DataMember]
        public Dictionary<string, object> Argument { get; set; }
    }
}