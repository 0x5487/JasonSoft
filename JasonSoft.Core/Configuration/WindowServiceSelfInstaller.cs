using System;
using System.Configuration.Install;
using System.Reflection;

namespace JasonSoft.Configuration
{
    public class WindowServiceSelfInstaller
    {
        private readonly String _exePath;

        public WindowServiceSelfInstaller(Assembly target)
        {
            if (target == null) throw new ArgumentNullException("target");

            _exePath = target.Location;
        }

        public bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(
                    new string[] { "/u", _exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
