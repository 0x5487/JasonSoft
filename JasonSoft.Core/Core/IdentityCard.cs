using System;
using System.Net;

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
