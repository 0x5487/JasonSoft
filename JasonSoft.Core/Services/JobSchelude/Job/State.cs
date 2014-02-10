using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public enum State
    {
        [EnumMember]
        Nothing = 0,

        [EnumMember]
        Disabled = 1,
        [EnumMember]
        Waiting = 2,
        [EnumMember]
        Running = 3,
        [EnumMember]

        Success = 4,
        [EnumMember]
        Failed = 5,
        [EnumMember]
        StopManually = 6
    }
}
