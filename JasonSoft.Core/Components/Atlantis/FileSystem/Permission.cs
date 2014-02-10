using System;

namespace JasonSoft.Components.Atlantis
{
    [Serializable]
    public class Permission 
    {
        public Permission()
        {
            AllowStatus = PermissionRightStatus.Empty;
            DenyStatus = PermissionRightStatus.Empty;
        }

        public Permission(PermissionEnum permissionEnum)
        {

        }

        public Int32 ID { get; set; }

        public Char Code { get; set; }

        public String Name { get; set; }

        public PermissionRightStatus AllowStatus { get; set; }

        public PermissionRightStatus DenyStatus { get; set; }
    }

    public enum PermissionEnum
    {
        FullControl,
        Edit,
        Delete,
        Write,
        Read,
        SetPermission,
        ReadPermission,
    }
}