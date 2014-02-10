using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using JasonSoft;
using JasonSoft.Reflection;
using JasonSoft.IO;
using Xunit;




namespace JasonSoft.Tests.Core.Extension
{

    public class ExtensionTestCase
    {
        [Fact]
        public void StringTest()
        {
            String target = "<abc>123456789</abc>";
            String result = target.Substring("<abc>", "</abc>");
            Assert.True(result == "123456789");

            target = "jason12345angle";
            result = target.Substring("jason", "angle");
            Assert.True(result == "12345");

            target = "nothing";
            result = target.Substring("a", "b");
            Assert.Null(result);

        }

        [Fact]
        public void IsNullOrWhiteSpace()
        {
            String str1 = "Hello, world!";
            String str2 = null;
            String str3 = string.Empty;
            String str4 = "\t\r\n ";

            Assert.False(str1.IsNullOrWhiteSpace());
            Assert.True(str2.IsNullOrWhiteSpace());
            Assert.True(str3.IsNullOrWhiteSpace());
            Assert.True(str4.IsNullOrWhiteSpace());
        }

        [Fact]
        public void StringSplitByKeyTest()
        {
            String target = "a: bcd e: fg h: i h:";
            Dictionary<String, String> answers = target.SplitByKey("a:|e:|h:", true);

            Assert.True(answers.Count == 3);

            String location = @"C:\WebPage\FortinetIDS\12113.html";
            FileInfo file = new FileInfo(location);
            String answer = file.GetNameWithoutExtension();

        }



        [Fact]
        public void ConvertUTCTime()
        {
            DateTime dateTime = new DateTime(2008, 10, 3, 13, 0, 0, 0, DateTimeKind.Unspecified);
            //TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            //DateTime newDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);
            DateTime newDateTime = dateTime.ConvertUTCTime("Taipei Standard Time");

            Assert.Same(21, newDateTime.Hour);
        }

        [Fact]
        public void JSONTest()
        {
            //List<Jason> collection = new List<Jason>();

            //Jason jason = new Jason() { Name = "李振維", Age = 20, };
            //Jason angela = new Jason() { Name = "Angela", Age = 20, };
            //collection.Add(jason);
            //collection.Add(angela);

            //String result = collection.ToJSON();
            //Console.WriteLine(result);

            SmallQ smallQ = new SmallQ();
            smallQ.Birthday = DateTime.Now;
            smallQ.Age = 28;
            String answer = smallQ.ToJSON();
   
        }

        [Fact]
        public void LoadTest()
        {
            Jason jason1 = new Jason();
            jason1.Name = "Jason";
            jason1.Age = 10;
            jason1.Birthday = new DateTime(1980,12,02);
            jason1.Members = "Jason";
            //jason1.Girl = new Angela() {Width = 10.25};


            FakeJason jason2 = new FakeJason();
            jason2.Load(jason1);
            Assert.Same(jason1.Name, jason2.Name);
            //Assert.Same(jason1.Birthday, jason2.Birthday);
            //Assert.Same(jason1.Girl.Width, jason2.Girl.Width);
        }

        [Fact]
        public void CombineTest()
        {
            List<String> words = new List<string>();
            words.Add("Jason");
            words.Add("Love");
            words.Add("Angela");

            String answer = words.Combine(",");

            Console.WriteLine(answer);
            Assert.Same("Jason,Love,Angela", answer);

            words = new List<string>();
            answer = words.Combine(String.Empty);
            Assert.Same(String.Empty, answer);
        }

        [Fact]
        public void ChangeTypeTest()
        {
            Int32? value = null;

            var an = value.ChangeTypeTo<Int32?>();
            Assert.Null(an);

            string aa = "12";
            Type type = aa.GetType();
            Console.WriteLine(type.IsPrimitive);
        }

        [Fact]
        public void ContainTest()
        {
            String str = "abCdefijk";
            Boolean result = str.Contain("cd");
            Assert.True(result);

            str = "李振維";
            result = str.Contain("維");
            Assert.True(result);
        }

        [Fact]
        public void EnumConverter()
        {
            String aa = "Modified";
            ObjectStatus state = aa.ToEnum<ObjectStatus>();
        }
    }
}

[Serializable]
[DataContract]
public class Jason
{

    [DataMember]
    public String Name { get; set; }

    [DataMember]
    public Int64 Age { get; set; }

    [DataMember]
    public DateTime Birthday { get; set; }

    public Angela Girl { get; set; }

    public String Members { get; set; }
}

[Serializable]
[DataContract]
public class SmallQ : Jason
{
    [DataMember(Name = "NewAge")]
    public new Int32 Age
    {
        get { return (Int32)base.Age; }
        set { base.Age = value; }

    }
}

public class FakeJason
{
    [DataMember]
    public String Name { get; set; }

    [DataMember]
    public Int32 Age { get; set; }

    [DataMember]
    public DateTime? Birthday { get; set; }

    public Angela Girl { get; set; }

    public List<String> Members { get; set; }
}

public class Angela
{
    public Double Width { get; set; }

}


