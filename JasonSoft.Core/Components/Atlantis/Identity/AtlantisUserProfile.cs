using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Atlantis
{
    public class AtlantisUserProfile
    {
        public AtlantisUserProfile()
        {
            
        }

        public AtlantisUserProfile(AtlantisUserProfileEnum key)
        {
            
        }

        public Int32 UserID { get; set; }

        public String UserName { get; set; }

        public String DisplayName { get; set; }

        public String Description { get; set; }

        public String Email { get; set; }

        public Guid MembershipID { get; set; }


    }

    public enum AtlantisUserProfileEnum
    {
        Administrator,
        System,
    }
}
