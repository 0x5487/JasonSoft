using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using JasonSoft.Microsoft;
using JasonSoft.Microsoft.IIS;
using JasonSoft.IO;
using Xunit;



namespace JasonSoft.Tests.Microsoft
{

    public class IISTest
    {
        [Fact]
        public void GetIIsWebSite()
        {
            IIsService iisService = new IIsService();
            IIsSite[] website = iisService.Sites;
        }

        [Fact]
        public void CreateIIsVirtualDirectory()
        {
            IIsService iisService = new IIsService();
            IIsSite website = iisService.Sites[2];

            using (IIsVirtualDirectory virtualDirectory = website.AddVirtualDirectory("UGuardJ", @"C:\Inetpub\wwwroot\abc\"))
            {
                virtualDirectory.CreateApplication();
                virtualDirectory.AccessPermissions = AccessPermissionFlags.Read + (int)AccessPermissionFlags.Script;
            }
        }

        [Fact]
        public void CheckWebSiteHealth()
        {
            IIsService iisService = new IIsService();
            IIsSite website = iisService.Sites[2];

            website.VirtualDirectories[0].ApplicationPoolId = "UGuardNotificationServer";
            

        }

        [Fact]
        public void SetNTFSPermission()
        {
            DirectoryInfo dir = new DirectoryInfo(@"C:\Inetpub\abc\");
            dir.SetNtfsPermission("NETWORK SERVICE", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);

            String iuser = "IUSR_" + System.Environment.MachineName;
            dir.SetNtfsPermission(iuser, FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);

            
        }

        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights,
                                        InheritanceFlags Inheritance, PropagationFlags Propogation,
                                        AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object. 
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            // Get a DirectorySecurity object that represents the  
            // current security settings. 
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            // Add the FileSystemAccessRule to the security settings.  
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account,
                                                             Rights,
                                                             Inheritance,
                                                             Propogation,
                                                             ControlType));

            

            
            // Set the new access settings. 
            dInfo.SetAccessControl(dSecurity);
        }

        [Fact]
        public void CreateNewWebSite()
        {
            IIsService iisService = new IIsService();
            using (IIsSite newWebSite = iisService.AddSite("dd", @"C:\Inetpub\abc\", "Testing.com", 110, String.Empty))
            {
                newWebSite.ASPNETVersion = ASPNETVersions.v2_0_50727;

                var newPool = iisService.AddAppPool("testpool");

                newWebSite.ApplicationPoolId = newPool.Id;

                newWebSite.StartSite(true);
            }

        }

        [Fact]
        public void CheckService()
        {
            string error = null;
            bool ans = ServiceHelper.IsServiceStopped("UManager Service", out error);
            Console.Write(ans);
        }

    }
}
