using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JasonSoft;
using JasonSoft.Math;
using Xunit;


namespace JasonSoft.Tests.Core
{
  
    public class HelperTestCase
    {
        [Fact]
        public void GetStringRandonTest()
        {
            String result = StringHelper.GetRandom(8);
            Console.WriteLine(result);
            Assert.True( result.Length == 8);
        }

        [Fact]
        public void GetNumberRadonTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Int32 result = 48.Random(122);
                Console.Write(result.ToString() + ",");
            }
            
        }

        [Fact]
        public void EncryptAndDecrypt()
        {
            String target = "Jason Lee";
            String encryption = EncryptHelper.Encrypt(target);
            String dncryption = EncryptHelper.Decrypt(encryption);

            Assert.Equal<String>(target, dncryption);
        }

        [Fact]
        public void ApplicationHelper()
        {
            CLRVersion aa = JasonSoft.ApplicationHelper.RunOnClr();
            Console.WriteLine(aa);
        }
    }
}
