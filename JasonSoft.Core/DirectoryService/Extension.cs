using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace JasonSoft.DirectoryService
{
    public static class DirectoryServiceExtension
    {
        public static bool AccountExists(this String name)
        {
            bool bRet = false;

            try
            {
                NTAccount acct = new NTAccount(name);
                SecurityIdentifier id = (SecurityIdentifier)acct.Translate(typeof(SecurityIdentifier));

                bRet = id.IsAccountSid();
            }
            catch (IdentityNotMappedException)
            {
                /* Invalid user account */
            }

            return bRet;
        }

        public static bool IsUserGroupExist(this String groupName)
        {
            try
            {
                DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
                DirectoryEntry userGroup = localMachine.Children.Find(groupName, "group");
                return true;
            }
            catch
            {

            }

            return false;
        }

        public static bool IsUserInGroup(this string userName, string groupName)
        {
            bool ret = false;

            try
            {
                DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
                DirectoryEntry userGroup = localMachine.Children.Find(groupName, "group");

                object members = userGroup.Invoke("members", null);
                foreach (object groupMember in (IEnumerable)members)
                {
                    DirectoryEntry member = new DirectoryEntry(groupMember);
                    if (member.Name.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        ret = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
    }
}
