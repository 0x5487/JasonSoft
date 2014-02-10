using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Atlantis
{
    public class FileSystemManager
    {
        private AtlantisUserProfile _executer;

        public FileSystemManager(AtlantisUserProfile executer)
        {
            _executer = executer;
        }



        public FileSystem GetFileSystem(String name)
        {
            throw new NotImplementedException();
        }


        public static void CreateFileSystem(String name, String rootNodeName)
        {
            throw new NotImplementedException();
        }

        public static void CreateFileSystem(String name, String rootNodeName, String defaultPermissionGroup)
        {
            throw new NotImplementedException();
        }

        public static void DeleteFileSystem(String name)
        {
            throw new NotImplementedException();
        }


        public static Permission GetPermission(Char code)
        {
            throw new NotImplementedException();
        }

        public static Permission GetPermission(PermissionEnum permissionEnum)
        {
            throw new NotImplementedException();
        }

        public static Permission[] GetPermissions()
        {
            throw new NotImplementedException();
        }

        public static Permission[] GetPermissionsFromGroup(String permissionGroupName)
        {
            throw new NotImplementedException();
        }

        public static Permission CreatePermission(Permission permission)
        {
            throw new NotImplementedException();
        }

        public static void DeletePermission(Int32 itemID)
        {
            throw new NotImplementedException();
        }

        public static void UpdatePermission(Permission permission)
        {
            throw new NotImplementedException();
        }

        public static String[] GetPermissionGroups()
        {
            throw new NotImplementedException();
        }

        public static void CreatePermissionGroup(String permissionGroupName)
        {
            throw new NotImplementedException();
        }

        public static void DeletePermissionGroup(String permissionGroupName)
        {
            throw new NotImplementedException();
        }

        public static void ClearPermissionGroups()
        {
            throw new NotImplementedException();
        }

        public static void ClearPermissions()
        {
            throw new NotImplementedException();
        }

        public static void UpdatePermissionGroup(String oldPermissionGroupName, String newPermissionGroupName)
        {
            throw new NotImplementedException();
        }

        public static void AddPermissionToGroup(Int32 permissionID, String permissionGroupName)
        {
            throw new NotImplementedException();
        }

        public static void RemovePermissionFromGroup(Int32 permissionItemID, String permissionGroupName)
        {
            throw new NotImplementedException();
        }

        public static void UpdateUserOrGroupDisplayName(Int32 userID, String newDisplayName)
        {
            throw new NotImplementedException();
        }
    }
}
