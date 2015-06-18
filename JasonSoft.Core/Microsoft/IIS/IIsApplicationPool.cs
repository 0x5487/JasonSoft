using System;
using System.DirectoryServices;
using System.Runtime.InteropServices;

namespace JasonSoft.Microsoft.IIS
{


    public class IIsApplicationPool : IDisposable
    {

        #region "Privates"

        private DirectoryEntry _ApplicationPoolEntry;

        #endregion

        #region "Constructors and Destructors"

        public IIsApplicationPool(DirectoryEntry ApplicationPoolEntry)
        {

            if (ApplicationPoolEntry.SchemaClassName == IIsHelper.ApplicationPoolSchemaClassName)
            {

                _ApplicationPoolEntry = ApplicationPoolEntry;
                _ApplicationPoolEntry.RefreshCache();
            }

            else
            {

                IIsHelper.ConstructorEx(IIsHelper.ApplicationPoolSchemaClassName);

            }

        }

        ~IIsApplicationPool()
        {

            if ((_ApplicationPoolEntry != null)) _ApplicationPoolEntry.Close();

        }

        public void Dispose()
        {

            if ((_ApplicationPoolEntry != null)) _ApplicationPoolEntry.Close();

        }

        #endregion

        #region "Properties"

        public string Id
        {


            get { return _ApplicationPoolEntry.Name; }
        }

        public AppPoolStates State
        {


            get { return (AppPoolStates)IIsHelper.GetProperty("AppPoolState", _ApplicationPoolEntry); }
        }

        public AppPoolIdentityTypes IdentityType
        {


            get { return (AppPoolIdentityTypes)IIsHelper.GetProperty("AppPoolIdentityType", _ApplicationPoolEntry); }


            set { IIsHelper.SetProperty("AppPoolIdentityType", value, _ApplicationPoolEntry); }
        }

        public string IdentityUserName
        {


            get { return IIsHelper.GetProperty("WAMUserName", _ApplicationPoolEntry).ToString(); }


            set { IIsHelper.SetProperty("WAMUserName", value, _ApplicationPoolEntry); }
        }

        public string IdentityPassword
        {


            get { return IIsHelper.GetProperty("WAMUserPass", _ApplicationPoolEntry).ToString(); }


            set { IIsHelper.SetProperty("WAMUserPass", value, _ApplicationPoolEntry); }
        }

        public bool AutoStart
        {


            get { return (Boolean)IIsHelper.GetProperty("AppPoolAutoStart", _ApplicationPoolEntry); }


            set { IIsHelper.SetProperty("AppPoolAutoStart", value, _ApplicationPoolEntry); }
        }

        public int IdleTimout
        {


            get { return (Int32)IIsHelper.GetProperty("IdleTimout", _ApplicationPoolEntry); }


            set { IIsHelper.SetProperty("IdleTimeout", value, _ApplicationPoolEntry); }
        }

        #endregion

        #region "Public Functions and Methods"

        public void StartPool([Optional, DefaultParameterValue(true)]  // ERROR: Optional parameters aren't supported in C#
bool WaitTillDone)
        {

            if (WaitTillDone)
            {

                _ApplicationPoolEntry.Invoke("Start", null);
            }

            else
            {

                IIsHelper.SetProperty("AppPoolCommand", 1, _ApplicationPoolEntry);

            }

            _ApplicationPoolEntry.RefreshCache();

        }

        public void StopPool([Optional, DefaultParameterValue(true)]  // ERROR: Optional parameters aren't supported in C#
bool WaitTillDone)
        {

            if (WaitTillDone)
            {

                _ApplicationPoolEntry.Invoke("Stop", null);
            }

            else
            {

                IIsHelper.SetProperty("AppPoolCommand", 2, _ApplicationPoolEntry);

            }

            _ApplicationPoolEntry.RefreshCache();

        }

        public void RecyclePool()
        {

            _ApplicationPoolEntry.Invoke("Recycle", null);

        }

        public string[] EnumAppsInPool()
        {

            object[] oApps = (Object[])_ApplicationPoolEntry.Invoke("EnumAppsInPool", null);
            string[] sApps = { };

            if (oApps.Length > 0)
            {

                // ERROR: Not supported in C#: ReDimStatement


                for (Int32 iApp = 0; iApp <= oApps.Length - 1; iApp++)
                {

                    sApps[iApp] = oApps[iApp].ToString();

                }

            }

            return sApps;

        }

        #endregion

    }

}
