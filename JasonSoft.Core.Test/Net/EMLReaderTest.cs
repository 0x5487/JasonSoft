using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using JasonSoft.Net;
using JasonSoft.IO;
using Xunit;

namespace JasonSoft.Tests.Net
{
    public class EMLReaderTest
    {
        [Fact]
        public void ReadEML()
        {
            string eml01 = @"C:\Temp\EML\1.eml";


            MailMessage testMail = MailMessageMimeParser.ParseMessage(eml01);
            //testMail.Attachments[7].ContentStream.WriteToFile(@"c:\temp\eml\11.png", true);

        }
    }
}
