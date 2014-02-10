using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using JasonSoft.Net;



namespace JasonSoft.Tests.Net
{
    public class IPAddressTest
    {
        [Fact]
        public void GetIP()
        {
            UInt32 ipNumber = 3232235521;
            System.Net.IPAddress aa = new System.Net.IPAddress(ipNumber);
            Console.WriteLine(aa.ToNumber());
        }
    }
}
