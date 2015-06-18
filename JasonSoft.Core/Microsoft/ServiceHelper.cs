using System;
using System.Management;

namespace JasonSoft.Microsoft
{
    public static class ServiceHelper
    {

        public static bool IsServiceStopped(string serviceName, out string errorMsg)
        {
            bool isStopped = false;
            errorMsg = string.Empty;

            string filter =
                String.Format("SELECT * FROM Win32_Service WHERE Name = '{0}'", serviceName);

            ManagementObjectSearcher query = new ManagementObjectSearcher(filter);

            // No match = stopped
            if (query == null) return true;

            try
            {
                ManagementObjectCollection services = query.Get();

                foreach (ManagementObject service in services)
                {
                    string currentStatus = Convert.ToString(service["State"]);
                    isStopped = (currentStatus.ToLower() == "stopped");
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                throw;
            }

            return isStopped;
        }
    }
}
