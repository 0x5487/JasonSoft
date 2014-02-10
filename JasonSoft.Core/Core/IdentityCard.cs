using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace JasonSoft
{
    public class IdentityCard
    {
        public Int32? UserID { get; set; }

        public string Username { get;  set; }

        public string[] Roles { get;  set; }

        public IPAddress IPAddress { get;  set; }

        public String TimeZoneID { get;  set; }

    }




}
