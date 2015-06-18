using System;
using System.Runtime.Serialization;

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
