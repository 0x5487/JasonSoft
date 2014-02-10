using System;

namespace JasonSoft.Components.Atlantis
{
    [Serializable]
    public enum PermissionRightStatus
    {
        Empty = 0,
        Inherit = 1,
        Native = 2,
        NativeAndInherit = 3,
    }
}