using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JasonSoft.Net;
using JasonSoft.XML;
using Xunit;

namespace JasonSoft.Tests.XML
{

    public class ExtensionMethodTestCase
    {
        [Fact]
        public void RemoveHTMLTag()
        {
            String source = "<a href='http://www.google.com'>google</a>Jason";
            Assert.Same("googleJason", source.RemoveHTMLTag());
        }

        [Fact]
        public void FormatXML()
        {
            String target = "<JasonSoft>  <Person>             <Name>Jason</Name></Person>   <Person>Angela</Person></JasonSoft>";
            Assert.True(@"<Person><Name>Jason</Name></Person><Person>Angela</Person>" == target.FormatXML(Formatting.None));
        }

        [Fact]
        public void ToAndFromXML()
        {
            String foo = "foo";

            String xmlFoo = foo.ToXML();
            Console.WriteLine(xmlFoo);

            String normalFoo = xmlFoo.FromXML<String>();
            Console.WriteLine(normalFoo);
        }
    }
}
