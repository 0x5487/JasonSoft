using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace JasonSoft.Microsoft.IIS
{


    public class IIsVirtualDirectory : IDisposable
    {

        #region "Privates"

        private DirectoryEntry _VirtualDirEntry;

        #endregion

        #region "Constructors and Destructors"

        public IIsVirtualDirectory(DirectoryEntry VirtualDirEntry)
        {

            if (VirtualDirEntry.SchemaClassName == IIsHelper.VirtualDirectorySchemaClassName)
            {

                _VirtualDirEntry = VirtualDirEntry;
                _VirtualDirEntry.RefreshCache();
            }

            else
            {

                IIsHelper.ConstructorEx(IIsHelper.VirtualDirectorySchemaClassName);

            }

        }

        ~IIsVirtualDirectory()
        {

            if ((_VirtualDirEntry != null)) _VirtualDirEntry.Close();

        }

        public void Dispose()
        {

            if ((_VirtualDirEntry != null)) _VirtualDirEntry.Close();

        }

        #endregion

        #region "Properties"

        public string Name
        {


            get { return _VirtualDirEntry.Name; }
        }

        public string Path
        {


            get { return IIsHelper.GetProperty("Path", _VirtualDirEntry).ToString(); }


            set { IIsHelper.SetProperty("Path", value, _VirtualDirEntry); }
        }


        public bool IsApplication
        {
            get
            {
                //_VirtualDirEntry.RefreshCache();

                String AppRoot = ((string)IIsHelper.GetProperty("AppRoot", _VirtualDirEntry)).Replace(IIsHelper.LocalMachineNamespace, IIsHelper.IIsProviderPath);

                return AppRoot == _VirtualDirEntry.Path ? true : false;

            }
        }

        public string ApplicationPoolId
        {


            get { return IIsHelper.GetProperty("AppPoolId", _VirtualDirEntry).ToString(); }
            set
            {

                if (this.IsApplication)
                {

                    IIsHelper.SetProperty("AppPoolId", value, _VirtualDirEntry);
                }

                else
                {

                    IIsHelper.SetPropertyEx("AppPoolId", new Exception("This is not an application. Can't set Application Pool."));

                }

            }
        }

        public string ApplicationName
        {


            get { return IIsHelper.GetProperty("AppFriendlyName", _VirtualDirEntry).ToString(); }
            set
            {

                if (this.IsApplication)
                {

                    IIsHelper.SetProperty("AppFriendlyName", value, _VirtualDirEntry);
                }

                else
                {

                    IIsHelper.SetPropertyEx("AppFriendlyName", new Exception("This is not an application. Can't set Application Name."));

                }

            }
        }

        public ASPNETVersions ASPNETVersion
        {


            get { return IIsHelper.GetScriptMaps(_VirtualDirEntry.Path); }
            set
            {

                if (this.IsApplication)
                {

                    IIsHelper.SetScriptMaps(_VirtualDirEntry.Path, value);
                }

                else
                {

                    IIsHelper.SetPropertyEx("ASPNETVersion", new Exception("This is not an application. Can't set ASP.NET Version."));

                }

            }
        }


        public DirectoryBrowseFlags DirectoryBrowsing
        {


            get { return (DirectoryBrowseFlags)IIsHelper.GetProperty("DirBrowseFlags", _VirtualDirEntry); }


            set { IIsHelper.SetProperty("DirBrowseFlags", value, _VirtualDirEntry); }
        }

        public string DefaultDocuments
        {


            get { return IIsHelper.GetProperty("DefaultDoc", _VirtualDirEntry).ToString(); }


            set { IIsHelper.SetProperty("DefaultDoc", value, _VirtualDirEntry); }
        }

        public AccessPermissionFlags AccessPermissions
        {


            get { return (AccessPermissionFlags)IIsHelper.GetProperty("AccessFlags", _VirtualDirEntry); }


            set { IIsHelper.SetProperty("AccessFlags", value, _VirtualDirEntry); }
        }

        public AuthenticationFlags AuthenticationMethods
        {


            get { return (AuthenticationFlags)IIsHelper.GetProperty("AuthFlags", _VirtualDirEntry); }


            set { IIsHelper.SetProperty("AuthFlags", value, _VirtualDirEntry); }
        }


        public IIsDirectory[] Directories
        {
            get {
            
            IIsDirectory[] results = new IIsDirectory[0];
            
            try {

                results = (from de in _VirtualDirEntry.Children.Cast<DirectoryEntry>()
                           where de.SchemaClassName == IIsHelper.DirectorySchemaClassName
                           select new IIsDirectory(de)).ToArray();
            }
            
            catch (Exception ex) {
                
                IIsHelper.GetPropertyEx("Directories", ex);
                
            }
            
            return results;
            
        }
        }

        public IIsVirtualDirectory[] VirtualDirectories
        {
            get {
            
            IIsVirtualDirectory[] results = new IIsVirtualDirectory[0];
            
            try {

                results = (from vde in _VirtualDirEntry.Children.Cast<DirectoryEntry>() 
                                        where vde.SchemaClassName == IIsHelper.VirtualDirectorySchemaClassName 
                                        select new IIsVirtualDirectory(vde)).ToArray();

            }
            
            catch (Exception ex) {
                
                IIsHelper.GetPropertyEx("VirtualDirectories", ex);
                
            }

            return results;
            
        }
        }

        public IIsFile[] Files
        {
            get {
            
            IIsFile[] Fs = new IIsFile[0];
            
            try {

                Fs = (from fe in _VirtualDirEntry.Children.Cast<DirectoryEntry>()
                                  where fe.SchemaClassName == IIsHelper.FileSchemaClassName 
                                  select new IIsFile(fe)).ToArray();

            }
            
            catch (Exception ex) {
                
                IIsHelper.GetPropertyEx("Files", ex);
                
            }
            
            return Fs;
            
        }
        }

        #endregion

        #region "Public Functions and Methods"

        public IIsDirectory AddDirectory(string Name)
        {
            IIsDirectory functionReturnValue = default(IIsDirectory);

            functionReturnValue = null;

            try
            {

                DirectoryEntry d = (DirectoryEntry)_VirtualDirEntry.Invoke("Create", IIsHelper.DirectorySchemaClassName, Name);

                d.CommitChanges();

                functionReturnValue = new IIsDirectory(d);

                d.Close();
            }

            catch (Exception ex)
            {

                throw new Exception(string.Format("An error occured while trying to add Directory '{0}'.", Name), ex);

            }
            return functionReturnValue;

        }

        public IIsVirtualDirectory AddVirtualDirectory(string Name, string Path)
        {
            IIsVirtualDirectory functionReturnValue = default(IIsVirtualDirectory);

            functionReturnValue = null;

            try
            {

                DirectoryEntry vd = (DirectoryEntry)_VirtualDirEntry.Invoke("Create", IIsHelper.VirtualDirectorySchemaClassName, Name);

                vd.CommitChanges();

                functionReturnValue = new IIsVirtualDirectory(vd);

                functionReturnValue.Path = Path;

                vd.Close();
            }

            catch (Exception ex)
            {

                throw new Exception(string.Format("An error occured while trying to add Virtual Directory '{0}'.", Name), ex);

            }
            return functionReturnValue;

        }

        public IIsFile AddFile(string Name)
        {
            IIsFile functionReturnValue = default(IIsFile);

            functionReturnValue = null;

            try
            {

                DirectoryEntry f = (DirectoryEntry)_VirtualDirEntry.Invoke("Create", IIsHelper.FileSchemaClassName, Name);

                f.CommitChanges();

                functionReturnValue = new IIsFile(f);

                f.Close();
            }

            catch (Exception ex)
            {

                throw new Exception(string.Format("An error occured while trying to add File '{0}'.", Name), ex);

            }
            return functionReturnValue;

        }

        public void CreateApplication()
        {
            CreateApplication(String.Empty, String.Empty);
        }

        public void CreateApplication(String applicationName, String applicationPoolID)
        {

            if (this.IsApplication)
            {

                throw new Exception("This is already an application.");
            }

            else
            {

                try
                {

                    _VirtualDirEntry.Invoke("AppCreate2", 2);

                    if (!string.IsNullOrEmpty(applicationName))
                    {

                        this.ApplicationName = applicationName;
                    }

                    else
                    {

                        this.ApplicationName = this.Name;

                    }

                    if (!string.IsNullOrEmpty(applicationPoolID)) this.ApplicationPoolId = applicationPoolID;
                }

                catch (Exception ex)
                {

                    throw new Exception("An error occured while trying to create an application.", ex);

                }

            }

        }

        public void DeleteApplication([System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute(false)]  // ERROR: Optional parameters aren't supported in C#
bool Recursive)
        {

            if (this.IsApplication)
            {

                try
                {

                    if (Recursive)
                    {

                        _VirtualDirEntry.Invoke("AppDeleteRecursive");
                    }

                    else
                    {

                        _VirtualDirEntry.Invoke("AppDelete");

                    }
                }

                catch (Exception ex)
                {

                    throw new Exception("An error occured while trying to delete an application.", ex);

                }
            }

            else
            {

                throw new Exception("This is not an application.");

            }

        }

        #endregion

    }

}
