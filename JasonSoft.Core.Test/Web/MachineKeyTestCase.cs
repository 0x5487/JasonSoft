using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using Xunit;


namespace JasonSoft.Tests.Web
{

    public class MachineKeyTestCase
    {
        [Fact]
        public void GetMachineKeyTest()
        {
            MachineKeySection machineKeySection = (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");
            //直接創建MachineKeySection的實例,ASP.NET 2.0中用machineKeySection取代ASP.NET 1.1中的MachineKey,並且可以直接訪問,沒有被internal保護。
            Type type = machineKeySection.GetType();

            PropertyInfo propertyInfo = type.GetProperty("ValidationKeyInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            Byte[] validationKeyArray = (Byte[]) propertyInfo.GetValue(machineKeySection, null);
            //獲取隨機生成的驗證密鑰字節數組

            propertyInfo = type.GetProperty("DecryptionKeyInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            Byte[] decryptionKeyArray = (Byte[])propertyInfo.GetValue(machineKeySection, null);
            //獲取隨機生成的加密密鑰字節數組

            MethodInfo byteArrayToHexString = type.GetMethod("ByteArrayToHexString", BindingFlags.Static | BindingFlags.NonPublic);
            //通過反射獲取MachineKeySection中的ByteArrayToHexString方法,該方法用於將字節數組轉換為16進製表示的字符串
            string validationKey = (string)byteArrayToHexString.Invoke(null, new object[] { validationKeyArray, validationKeyArray.Length });
            //將驗證密鑰字節數組轉換為16進製表示的字符串
            string DecryptionKey = (string)byteArrayToHexString.Invoke(null, new object[] { decryptionKeyArray, decryptionKeyArray.Length });
            //將加密密鑰字節數組轉換為16進製表示的字符串

            Console.WriteLine(validationKey);
            Console.WriteLine(DecryptionKey);
        }
    }
}
