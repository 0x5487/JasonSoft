using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Runtime.InteropServices;

using JasonSoft;

namespace JasonSoft.Microsoft.IIS
{
 

    public class IIsSite : IDisposable
    {

        #region "Privates"

        private DirectoryEntry _SiteEntry;
        private DirectoryEntry _RootEntry;

        #endregion

        #region "Constructors and Destructors"

        public IIsSite(DirectoryEntry SiteEntry, DirectoryEntry RootEntry)
        {

            if (SiteEntry.SchemaClassName == IIsHelper.SiteSchemaClassName & RootEntry.SchemaClassName == IIsHelper.VirtualDirectorySchemaClassName)
            {

                _SiteEntry = SiteEntry;
                _SiteEntry.RefreshCache();

                _RootEntry = RootEntry;
                _RootEntry.RefreshCache();
            }

            else
            {

                if (SiteEntry.SchemaClassName != IIsHelper.SiteSchemaClassName)
                {

                    IIsHelper.ConstructorEx(IIsHelper.SiteSchemaClassName);

                }

                if (RootEntry.SchemaClassName != IIsHelper.VirtualDirectorySchemaClassName)
                {

                    IIsHelper.ConstructorEx(IIsHelper.VirtualDirectorySchemaClassName);

                }

            }

        }

        ~IIsSite()
        {

            if ((_RootEntry != null)) _RootEntry.Close();
            if ((_SiteEntry != null)) _SiteEntry.Close();

        }

        public void Dispose()
        {

            if ((_RootEntry != null)) _RootEntry.Close();
            if ((_SiteEntry != null)) _SiteEntry.Close();

        }

        #endregion

        #region "Properties"

        public string Id
        {


            get { return _SiteEntry.Name; }
        }

        public SiteStates State
        {


            get { return (SiteStates)IIsHelper.GetProperty("ServerState", _SiteEntry); }
        }

        public bool AutoStart
        {


            get { return (Boolean)IIsHelper.GetProperty("ServerAutoStart", _SiteEntry); }


            set { IIsHelper.SetProperty("ServerAutoStart", value, _SiteEntry); }
        }



        public string Description
        {

            get { return IIsHelper.GetProperty("ServerComment", _SiteEntry).ToString(); }


            set { IIsHelper.SetProperty("ServerComment", value, _SiteEntry); }
        }

        public SiteBinding[] Bindings
        {
            get {
            
            String[] ObjectsA = IIsHelper.GetPropertyA("ServerBindings", _SiteEntry);
            SiteBinding[] BindingsA = null;
            
            if (ObjectsA.Length > 0) {
                
                
                for (Int32 wsbI = 0; wsbI <= ObjectsA.Length - 1; wsbI++) {
                    
                    String[] wsbA = ObjectsA[wsbI].Split(':');
                    
                    BindingsA[wsbI].IP = wsbA[0];
                    BindingsA[wsbI].Port = wsbA[1].ChangeTypeTo<int>();
                    BindingsA[wsbI].HostHeader = wsbA[2];
                    
                }
                
                return BindingsA;
            }
            
            else {
                
                return new SiteBinding[0];
                
            }
            
        }
            set {
            
            String[] ObjectsA = null;
            
            if (value.Length > 0) {
               

                
                for (Int32 wsbI = 0; wsbI <= value.Length - 1; wsbI++) {
                    
                    ObjectsA[wsbI] = string.Format("{0}:{1}:{2}", value[wsbI].IP, value[wsbI].Port, value[wsbI].HostHeader);
                    
                }
            }
            
            else {
                
                ObjectsA = new String[0];
                
            }
            
            IIsHelper.SetPropertyA("ServerBindings", ObjectsA, _SiteEntry);
            
        }
        }




        public string ApplicationPoolId
        {


            get { return IIsHelper.GetProperty("AppPoolId", _RootEntry).ToString(); }


            set { IIsHelper.SetProperty("AppPoolId", value, _RootEntry); }
        }

        public string ApplicationName
        {


            get { return IIsHelper.GetProperty("AppFriendlyName", _RootEntry).ToString(); }


            set { IIsHelper.SetProperty("AppFriendlyName", value, _RootEntry); }
        }

        public ASPNETVersions ASPNETVersion
        {


            get { return IIsHelper.GetScriptMaps(_RootEntry.Path); }


            set { IIsHelper.SetScriptMaps(_RootEntry.Path, value); }
        }


        public DirectoryBrowseFlags DirectoryBrowsing
        {


            get { return (DirectoryBrowseFlags)IIsHelper.GetProperty("DirBrowseFlags", _RootEntry); }


            set { IIsHelper.SetProperty("DirBrowseFlags", value, _RootEntry); }
        }

        public string DefaultDocuments
        {


            get { return IIsHelper.GetProperty("DefaultDoc", _RootEntry).ToString(); }


            set { IIsHelper.SetProperty("DefaultDoc", value, _RootEntry); }
        }

        public AccessPermissionFlags AccessPermissions
        {


            get { return (AccessPermissionFlags)IIsHelper.GetProperty("AccessFlags", _RootEntry); }


            set { IIsHelper.SetProperty("AccessFlags", value, _RootEntry); }
        }

        public AuthenticationFlags AuthenticationMethods
        {


            get { return (AuthenticationFlags)IIsHelper.GetProperty("AuthFlags", _RootEntry); }


            set { IIsHelper.SetProperty("AuthFlags", value, _RootEntry); }
        }

        public string Path
        {


            get { return IIsHelper.GetProperty("Path", _RootEntry).ToString(); }


            set { IIsHelper.SetProperty("Path", value, _RootEntry); }
        }


        public IIsDirectory[] Directories
        {
            get {
            
            IIsDirectory[] results = new IIsDirectory[0];
            
            try {

                foreach (var de in _RootEntry.Children.Cast<DirectoryEntry>())
                {
                 
                    String name = de.SchemaClassName; 
                }


                results = (from de in _RootEntry.Children.Cast<DirectoryEntry>()
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

                results = (from vde in _RootEntry.Children.Cast<DirectoryEntry>() 
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
                
                results = (from fe in _RootEntry.Children.Cast<DirectoryEntry>() 
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

        public void StartSite([System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute(true)]  // ERROR: Optional parameters aren't supported in C#
bool WaitTillDone)
        {

            if (WaitTillDone)
            {

                _SiteEntry.Invoke("Start", null);
            }

            else
            {

                IIsHelper.SetProperty("ServerCommand", 1, _SiteEntry);

            }

            _SiteEntry.RefreshCache();

        }

        public void StopSite([System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute(true)]  // ERROR: Optional parameters aren't supported in C#
bool WaitTillDone)
        {

            if (WaitTillDone)
            {

                _SiteEntry.Invoke("Stop", null);
            }

            else
            {

                IIsHelper.SetProperty("ServerCommand", 2, _SiteEntry);

            }

            _SiteEntry.RefreshCache();

        }

        public void PauseSite([System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute(true)]  // ERROR: Optional parameters aren't supported in C#
bool WaitTillDone)
        {

            if (WaitTillDone)
            {

                _SiteEntry.Invoke("Pause", null);
            }

            else
            {

                IIsHelper.SetProperty("ServerCommand", 3, _SiteEntry);

            }

            _SiteEntry.RefreshCache();

        }

        public void ContinueSite([System.Runtime.InteropServices.OptionalAttribute, System.Runtime.InteropServices.DefaultParameterValueAttribute(true)]  // ERROR: Optional parameters aren't supported in C#
bool WaitTillDone)
        {

            if (WaitTillDone)
            {

                _SiteEntry.Invoke("Continue", null);
            }

            else
            {

                IIsHelper.SetProperty("ServerCommand", 4, _SiteEntry);

            }

            _SiteEntry.RefreshCache();

        }

        public IIsDirectory AddDirectory(string Name)
        {
            IIsDirectory functionReturnValue = default(IIsDirectory);

            functionReturnValue = null;

            try
            {

                DirectoryEntry d = (DirectoryEntry)_RootEntry.Invoke("Create", IIsHelper.DirectorySchemaClassName, Name);

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

                DirectoryEntry vd = (DirectoryEntry)_RootEntry.Invoke("Create", IIsHelper.VirtualDirectorySchemaClassName, Name);

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

                DirectoryEntry f = (DirectoryEntry)_RootEntry.Invoke("Create", IIsHelper.FileSchemaClassName, Name);

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

        #endregion

    }

}
