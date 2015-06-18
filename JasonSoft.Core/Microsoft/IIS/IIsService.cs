using System;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices;

namespace JasonSoft.Microsoft.IIS
{


    public class IIsService : IDisposable
    {

        #region "Privates"

        private DirectoryEntry _ServiceEntry;
        private DirectoryEntry _ApplicationPoolsEntry;

        #endregion

        #region "Properties"

        public IIsSite[] Sites
        {
            get
            {

                IIsSite[] results = new IIsSite[0];

                try
                {

                    results = (from wse in _ServiceEntry.Children.Cast<DirectoryEntry>()
                               where wse.SchemaClassName == IIsHelper.SiteSchemaClassName
                               select new IIsSite(wse, new DirectoryEntry(wse.Path + "/Root"))).ToArray();


                }

                catch (Exception ex)
                {

                    IIsHelper.GetPropertyEx("Sites", ex);

                }

                return results;

            }
        }


        public IIsApplicationPool[] ApplicationPools
        {
            get
            {

                IIsApplicationPool[] results = new IIsApplicationPool[0];

                try
                {

                    results = (from ape in _ApplicationPoolsEntry.Children.Cast<DirectoryEntry>()
                               where ape.SchemaClassName == IIsHelper.ApplicationPoolSchemaClassName
                               select new IIsApplicationPool(ape)).ToArray();


                }

                catch (Exception ex)
                {

                    IIsHelper.GetPropertyEx("ApplicationPools", ex);

                }

                return results;

            }
        }

        #endregion

        #region "Constructors and Destructors"

        public IIsService()
        {

            String ServiceName = FirstServiceName();
            String AppPoolsName = FirstAppPoolsName();

            if (ServiceName == string.Empty)
            {

                throw new Exception("No IIS Web Service Metabase entry found on this machine.");
            }

            else
            {

                _ServiceEntry = new DirectoryEntry(IIsHelper.IIsProviderPath + "/" + ServiceName);
                _ServiceEntry.RefreshCache();

            }

            if (AppPoolsName == string.Empty)
            {

                throw new Exception("No Application Pools Metabase entry found on this machine.");
            }

            else
            {

                _ApplicationPoolsEntry = new DirectoryEntry(IIsHelper.IIsProviderPath + "/" + ServiceName + "/" + AppPoolsName);
                _ApplicationPoolsEntry.RefreshCache();

            }

        }

        ~IIsService()
        {

            if ((_ApplicationPoolsEntry != null)) _ApplicationPoolsEntry.Close();
            if ((_ServiceEntry != null)) _ServiceEntry.Close();

        }

        public void Dispose()
        {

            if ((_ApplicationPoolsEntry != null)) _ApplicationPoolsEntry.Close();
            if ((_ServiceEntry != null)) _ServiceEntry.Close();

        }

        #endregion

        #region "Private Functions and Methods"

        private string FirstServiceName()
        {

            DirectoryEntry IIsEntry = null;
            String ServiceName = "";

            try
            {

                IIsEntry = new DirectoryEntry(IIsHelper.IIsProviderPath);

                ServiceName = (from de in IIsEntry.Children.Cast<DirectoryEntry>()
                               where de.SchemaClassName == IIsHelper.ServiceSchemaClassName
                               select de.Name).FirstOrDefault();
            }

            catch
            {
            }

            finally
            {

                if ((IIsEntry != null)) IIsEntry.Close();

            }

            return ServiceName;

        }

        private string FirstAppPoolsName()
        {

            DirectoryEntry IIsEntry = null;
            String AppPoolsName = "";

            try
            {

                IIsEntry = new DirectoryEntry(IIsHelper.IIsProviderPath + "/" + FirstServiceName());

                AppPoolsName = (from de in IIsEntry.Children.Cast<DirectoryEntry>()
                                where de.SchemaClassName == IIsHelper.ApplicationPoolsSchemaClassName
                                select de.Name).FirstOrDefault();
            }

            catch
            {
            }

            finally
            {

                if ((IIsEntry != null)) IIsEntry.Close();

            }

            return AppPoolsName;

        }

        #endregion

        #region "Public Functions and Methods"

        public IIsApplicationPool AddAppPool(string Id)
        {
            IIsApplicationPool functionReturnValue = default(IIsApplicationPool);

            functionReturnValue = null;

            try
            {

                DirectoryEntry ae = (DirectoryEntry)_ApplicationPoolsEntry.Invoke("Create", IIsHelper.ApplicationPoolSchemaClassName, Id);

                ae.CommitChanges();

                functionReturnValue = new IIsApplicationPool(ae);

                ae.Close();
            }

            catch (Exception ex)
            {

                throw new Exception(string.Format("An error occured while trying to add Application Pool '{0}'.", Id), ex);

            }
            return functionReturnValue;

        }

        public bool RemoveAppPool(string Id)
        {

            try
            {

                _ApplicationPoolsEntry.Invoke("Delete", IIsHelper.ApplicationPoolSchemaClassName, Id);

                return true;
            }

            catch
            {

                return false;

            }

        }

        public IIsSite AddSite(string Description, string Path, SiteBinding[] Bindings)
        {

            try
            {

                Object[] ObjectsA = null;

                if (Bindings.Length > 0)
                {

                    ObjectsA = new Object[Bindings.Length];

                    for (Int32 wsbI = 0; wsbI <= Bindings.Length - 1; wsbI++)
                    {

                        ObjectsA[wsbI] = String.Format("{0}:{1}:{2}", Bindings[wsbI].IP, Bindings[wsbI].Port, Bindings[wsbI].HostHeader);

                    }
                }

                else
                {

                    ObjectsA = new Object[0];

                }

                //ObjectsA = new object[] { "Testing.com", new Object[]{":110:Testing.com"}, @"C:\Inetpub\abc\" };
                string SiteId = _ServiceEntry.Invoke("CreateNewSite", Description, ObjectsA, Path).ToString();

                DirectoryEntry ws = new DirectoryEntry(_ServiceEntry.Path + "\\" + SiteId);
                DirectoryEntry wr = new DirectoryEntry(ws.Path + "\\Root");

                return new IIsSite(ws, wr);
            }

            catch
            {

                return null;

            }

        }

        public IIsSite AddSite(string Description, string Path, SiteBinding Binding)
        {

            return AddSite(Description, Path, new SiteBinding[] { Binding });

        }

        public IIsSite AddSite(string Description, string Path, string BindingHostHeader, [Optional, DefaultParameterValue(80)]  // ERROR: Optional parameters aren't supported in C#
int BindingPort, [Optional, DefaultParameterValue("")]  // ERROR: Optional parameters aren't supported in C#
string BindingIP)
        {

            return AddSite(Description, Path, new SiteBinding { IP = BindingIP, Port = BindingPort, HostHeader = BindingHostHeader });

        }

        public bool RemoveSite(string Id)
        {

            try
            {

                _ServiceEntry.Invoke("Delete", IIsHelper.SiteSchemaClassName, Id);

                return true;
            }

            catch
            {

                return false;

            }

        }

        #endregion

    }

}
