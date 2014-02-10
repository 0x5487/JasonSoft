using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft.Tests.Core
{
    class SimpleObject
    {
        public String fieldString = "myFieldString";

        public SimpleObject()
        {
            MyString = "Hello World!!";
        }

        
        public String MyString { get; set; }
        public Int32 MyInt32 { get; set; }
        public DateTime MyDateTime { get; set; }
        public Double MyDouble { get; set; }
        public Single MySingle { get; set; }
        public Byte MyByte { get; set; }
        public Int16 MyInt16 { get; set; }
        public RefObject MyRefObject { get; set; }
    
    }

    class RefObject 
    {
    
    }
}
