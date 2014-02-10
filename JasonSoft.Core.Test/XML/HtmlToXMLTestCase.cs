using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using JasonSoft.Net;
using JasonSoft.XML;
using JasonSoft;
using Xunit;


namespace JasonSoft.Tests.XML
{

    public class HtmlToXMLTestCase
    {

        [Fact]
        public void LinqXML()
        {
            String target = "<JasonSoft>aa   <Person>             <Name>  J'as'on</Name></Person>   <Person>Angela</Person></JasonSoft>";

            StringReader stringReader = new StringReader(target.ToXHTML());

            XDocument xmlDoc = XDocument.Load(stringReader);
           

            var results = from p in xmlDoc.Elements("jasonSoft").Elements() select p;

            foreach(var result in results)
            {
                Console.WriteLine(result.ToString());
            }
        }
    }
}
