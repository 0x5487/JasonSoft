using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    public enum Mode
    {
        [EnumMember]
        Automatic = 0,
        [EnumMember]
        Manual = 1
    }
}
