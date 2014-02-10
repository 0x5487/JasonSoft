using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace JasonSoft.Microsoft.IIS
{
    public class IIsFile : IDisposable
    {

        #region "Privates"

        private DirectoryEntry _FileEntry;

        #endregion

        #region "Constructors and Destructors"

        public IIsFile(DirectoryEntry FileEntry)
        {

            if (FileEntry.SchemaClassName == IIsHelper.FileSchemaClassName)
            {

                _FileEntry = FileEntry;
                _FileEntry.RefreshCache();
            }
            else
            {
                IIsHelper.ConstructorEx(IIsHelper.FileSchemaClassName);
            }
        }

        ~IIsFile()
        {

            if ((_FileEntry != null)) _FileEntry.Close();

        }

        public void Dispose()
        {

            if ((_FileEntry != null)) _FileEntry.Close();

        }

        #endregion

        #region "Properties"

        public string Name
        {
            get { return _FileEntry.Name; }
        }

        public AccessPermissionFlags AccessPermissions
        {
            get { return (AccessPermissionFlags)IIsHelper.GetProperty("AccessFlags", _FileEntry); }

            set { IIsHelper.SetProperty("AccessFlags", value, _FileEntry); }
        }

        public AuthenticationFlags AuthenticationMethods
        {
            get { return (AuthenticationFlags)IIsHelper.GetProperty("AuthFlags", _FileEntry); }

            set { IIsHelper.SetProperty("AuthFlags", value, _FileEntry); }
        }

        #endregion

    }

}
