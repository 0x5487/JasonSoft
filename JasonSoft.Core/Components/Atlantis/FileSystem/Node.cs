using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Atlantis
{
    public class Node
    {
        public Int32 ID { get; internal set; }

        public Int32 ParentID { get; set; }

        public String Name { get; set; }

        public String Attribute { get; set; }

        public Boolean HasChild { get; internal set; }

        public Boolean IsInheritance { get; internal set; }

        public Int32 Level { get; internal set; }

        public Int32 RefObjectID { get; set; }

        public String RefObjectType { get; set; }

        public DateTime CreateDate { get; set; }

        public Int32 CreatorID { get; internal set; }

        public String CreatorName { get; internal set; }

        public DateTime LastModifiedDate { get; internal set; }

        public int LastModifierID { get; internal set; }

        public string LastModifierName { get; internal set; }
    }
}
