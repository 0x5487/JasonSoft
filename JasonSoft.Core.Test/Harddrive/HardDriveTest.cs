using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using Xunit;


namespace JasonSoft.Tests.Harddrive
{

    public class HardDriveTest
    {
        [Fact]
        public void GetFreeSpace()
        {
            SelectQuery query = new SelectQuery("select name, FreeSpace from win32_logicaldisk WHERE deviceID = 'C:'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                Console.WriteLine("Drive letter is: {0}", mo["name"]);
                Console.WriteLine("Drive's free space is: {0}", mo["FreeSpace"]);
            }

            // Here to stop app from closing
            Console.WriteLine("\nPress Return to exit.");
            Console.Read();
        }
    }
}
