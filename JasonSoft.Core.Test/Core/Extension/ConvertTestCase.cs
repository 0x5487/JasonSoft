using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace JasonSoft.Tests
{
    public class ConvertTestCase
    {
        [Fact]
        public void ConvertTest()
        {
            //convert int
            string i = "5";
            int j = i.ChangeTypeTo<int>();
            Assert.Equal(5, 5);

            //convert bool
            string strBoolean = "true";
            bool boolean = strBoolean.ChangeTypeTo<bool>();
            Assert.Equal(true, boolean);

            //convert datetime
            string strDate = "2010/10/15";
            DateTime date = strDate.ChangeTypeTo<DateTime>();
            Assert.Equal(new DateTime(2010,10,15), date);

            //convert guid
            Guid guid = Guid.NewGuid();
            string strGUID = guid.ToString();
            Guid newGUID = guid.ChangeTypeTo<Guid>();
            Assert.Equal(guid, newGUID);

        }

        [Fact]
        public void ConvertPerformanceTest()
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                string strInt = "345678";
                int myint = int.Parse(strInt);
            }
            watch.Stop();
            Console.WriteLine(string.Format("nomral: {0}", watch.Elapsed));

            watch.Reset();
            watch.Start();

            for (int i = 0; i < 1000000; i++)
            {
                string strInt = "345678";
                int myint = strInt.ChangeTypeTo<int>();
            }

            watch.Stop();
            Console.WriteLine(string.Format("use changeType: {0}", watch.Elapsed));

        }
    }
}
