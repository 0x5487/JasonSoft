using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

namespace JasonSoft.Microsoft.IIS
{

    public class IIsDirectory : IDisposable
    {

        #region "Privates"

        private DirectoryEntry _DirectoryEntry;

        #endregion

        #region "Constructors and Destructors"

        public IIsDirectory(DirectoryEntry DirEntry)
        {

            if (DirEntry.SchemaClassName == IIsHelper.DirectorySchemaClassName)
            {

                _DirectoryEntry = DirEntry;
                _DirectoryEntry.RefreshCache();
            }

            else
            {

                IIsHelper.ConstructorEx(IIsHelper.DirectorySchemaClassName);

            }

        }

        ~IIsDirectory()
        {

            if ((_DirectoryEntry != null)) _DirectoryEntry.Close();

        }

        public void Dispose()
        {

            if ((_DirectoryEntry != null)) _DirectoryEntry.Close();

        }

        #endregion

        #region "Properties"

        public string Name
        {


            get { return _DirectoryEntry.Name; }
        }


        public bool IsApplication
        {
            get
            {

                String AppRoot = ((string)IIsHelper.GetProperty("AppRoot", _DirectoryEntry)).Replace(IIsHelper.LocalMachineNamespace, IIsHelper.IIsProviderPath);

                return AppRoot == _DirectoryEntry.Path ? true : false;

            }
        }

        public string ApplicationPoolId
        {


            get { return IIsHelper.GetProperty("AppPoolId", _DirectoryEntry).ToString(); }
            set
            {

                if (this.IsApplication)
                {

                    IIsHelper.SetProperty("AppPoolId", value, _DirectoryEntry);
                }

                else
                {

                    IIsHelper.SetPropertyEx("AppPoolId", new Exception("This is not an application. Can't set Application Pool."));

                }

            }
        }

        public string ApplicationName
        {


            get { return IIsHelper.GetProperty("AppFriendlyName", _DirectoryEntry).ToString(); }
            set
            {

                if (this.IsApplication)
                {

                    IIsHelper.SetProperty("AppFriendlyName", value, _DirectoryEntry);
                }

                else
                {

                    IIsHelper.SetPropertyEx("AppFriendlyName", new Exception("This is not an application. Can't set Application Name."));

                }

            }
        }

        public ASPNETVersions ASPNETVersion
        {


            get { return IIsHelper.GetScriptMaps(_DirectoryEntry.Path); }
            set
            {

                if (this.IsApplication)
                {

                    IIsHelper.SetScriptMaps(_DirectoryEntry.Path, value);
                }

                else
                {

                    IIsHelper.SetPropertyEx("ASPNETVersion", new Exception("This is not an application. Can't set ASP.NET Version."));

                }

            }
        }


        public Object DirectoryBrowsing
        {


            get 
            { 
                return IIsHelper.GetProperty("DirBrowseFlags", _DirectoryEntry); 
            }


            set { IIsHelper.SetProperty("DirBrowseFlags", value, _DirectoryEntry); }
        }

        public string DefaultDocuments
        {


            get { return IIsHelper.GetProperty("DefaultDoc", _DirectoryEntry).ToString(); }


            set { IIsHelper.SetProperty("DefaultDoc", value, _DirectoryEntry); }
        }

        public AccessPermissionFlags AccessPermissions
        {


            get { return (AccessPermissionFlags)IIsHelper.GetProperty("AccessFlags", _DirectoryEntry); }


            set { IIsHelper.SetProperty("AccessFlags", value, _DirectoryEntry); }
        }

        public AuthenticationFlags AuthenticationMethods
        {


            get { return (AuthenticationFlags)IIsHelper.GetProperty("AuthFlags", _DirectoryEntry); }


            set { IIsHelper.SetProperty("AuthFlags", value, _DirectoryEntry); }
        }


        public IIsDirectory[] Directories
        {
            get {


                IIsDirectory[] directories = null;

            
            try {

                directories = (from de in _DirectoryEntry.Children.Cast<DirectoryEntry>()
                                 where de.Name == IIsHelper.DirectorySchemaClassName
                                 select new IIsDirectory(de)).ToArray();

                

            }
            
            catch (Exception ex) {
                
                IIsHelper.GetPropertyEx("Directories", ex);
                
            }

            return directories;
            
            }
        }

        public IIsVirtualDirectory[] VirtualDirectories
        {
            get {
            
            IIsVirtualDirectory[] results = null;
            
            try {
                
                results = (from vde in _DirectoryEntry.Children.Cast<DirectoryEntry>() 
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
            
            IIsFile[] results = new IIsFile[0];
            
            try {
                
                results = (from fe in _DirectoryEntry.Children.Cast<DirectoryEntry>()
                                  where fe.SchemaClassName == IIsHelper.FileSchemaClassName
                                  select new IIsFile(fe)).ToArray();
                
            }
            
            catch (Exception ex) {
                
                IIsHelper.GetPropertyEx("Files", ex);
                
            }
            
            return results;
            
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

                DirectoryEntry d = (DirectoryEntry)_DirectoryEntry.Invoke("Create", IIsHelper.DirectorySchemaClassName, Name);

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

                DirectoryEntry vd = (DirectoryEntry)_DirectoryEntry.Invoke("Create", IIsHelper.VirtualDirectorySchemaClassName, Name);

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

                DirectoryEntry f = (DirectoryEntry)_DirectoryEntry.Invoke("Create", IIsHelper.FileSchemaClassName, Name);

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

        public void CreateApplication([System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute("")]  // ERROR: Optional parameters aren't supported in C#
string ApplicationName, [System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute("")]  // ERROR: Optional parameters aren't supported in C#
string ApplicationPoolId)
        {

            if (this.IsApplication)
            {

                throw new Exception("This is already an application.");
            }

            else
            {

                try
                {

                    _DirectoryEntry.Invoke("AppCreate2", 2);

                    if (!string.IsNullOrEmpty(ApplicationName))
                    {

                        this.ApplicationName = ApplicationName;
                    }

                    else
                    {

                        this.ApplicationName = this.Name;

                    }

                    if (!string.IsNullOrEmpty(ApplicationPoolId)) this.ApplicationPoolId = ApplicationPoolId;
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

                        _DirectoryEntry.Invoke("AppDeleteRecursive");
                    }

                    else
                    {

                        _DirectoryEntry.Invoke("AppDelete");

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
