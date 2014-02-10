using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JasonSoft
{
    [Serializable]
    [DataContract]
    public enum ObjectStatus
    {
        [EnumMember]
        New = 0,
        [EnumMember]
        Normal = 1,
        [EnumMember]
        Modified = 2,
        [EnumMember]
        Deleted = 3,
        [EnumMember]
        Drop = 4
    }
}
