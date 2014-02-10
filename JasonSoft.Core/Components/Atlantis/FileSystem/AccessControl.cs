using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JasonSoft;

namespace JasonSoft.Components.Atlantis
{
    public class AccessControl
    {
        public AccessControl()
        {
            Status = ObjectStatus.New;
        }

        public Int32 ID { get; set; }

        public Int32 NodeID { get; set; }

        public Boolean IsInherited { get; set; }

        public Permission[] Permissions { get; set; }

        public UserGroupType UserGroupType { get; set; }

        public Int32 UserOrGroupID { get; set; }

        public ObjectStatus Status { get; set; }

        public string UserOrGroupName { get; set; }

        public bool CanDelete
        {
            get
            {
                bool result = true;

                foreach (var permission in Permissions)
                {
                    result = !IsPermissionInherited(permission.AllowStatus);
                    if (result == false)
                        return false;

                    result = !IsPermissionInherited(permission.DenyStatus);
                    if (result == false)
                        return false;
                }

                return result;
            }
        }

        public bool CanRemoveInheritedPermission
        {
            get
            {
                bool result = false;

                foreach (var permission in Permissions)
                {
                    if (permission.AllowStatus == PermissionRightStatus.NativeAndInherit || permission.DenyStatus == PermissionRightStatus.NativeAndInherit)
                    {
                        return true;
                    }
                }

                return result;
            }
        }

        public void RemoveInheritedPermission()
        {
            foreach (var permission in Permissions)
            {
                if (permission.AllowStatus == PermissionRightStatus.NativeAndInherit)
                {
                    permission.AllowStatus = PermissionRightStatus.Inherit;
                }

                if (permission.DenyStatus == PermissionRightStatus.NativeAndInherit)
                {
                    permission.DenyStatus = PermissionRightStatus.Inherit;
                }
            }
        }

        private bool IsPermissionInherited(PermissionRightStatus status)
        {
            bool result = false;

            switch (status)
            {
                case PermissionRightStatus.Inherit:
                case PermissionRightStatus.NativeAndInherit:
                    result = true;
                    break;
            }

            return result;
        }



    }
}
