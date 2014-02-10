using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Atlantis
{
    public class AtlantisUserGroup
    {
        public AtlantisUserGroup()
        {
            
        }

        public AtlantisUserGroup(AtlantisUserGroupEnum key)
        {

        }

        public Int32 ID { get; set; }
        
        public String Name { get; set; }

        public String Description { get; set; }

    }

    public enum AtlantisUserGroupEnum
    {
        Administrators,
        Users
    }
}
