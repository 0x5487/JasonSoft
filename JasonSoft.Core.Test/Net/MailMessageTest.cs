using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
using JasonSoft.Net;
using JasonSoft.IO;
using System.IO;

namespace JasonSoft.Tests.Net
{

    public class MailMessageTest
    {
        [Fact]
        public void SavePlain()
        {
            MailMessage testMail = new MailMessage();
            testMail.To.Add(new MailAddress("jawc@domain.com"));
            testMail.From = new MailAddress("sender@domain.com");
            testMail.Subject = "I am subject";
            testMail.Body = "Jason Lee";

            testMail.Attachments.Add(new Attachment(@"c:\temp\slide.jpg", MediaTypeNames.Image.Jpeg));
            testMail.Save(@"c:\Temp\plain.eml");
            
        }

        [Fact]
        public void SaveHtml()
        {
            MailMessage testMail = new MailMessage();
            testMail.To.Add(new MailAddress("jawc@domain.com"));
            testMail.From = new MailAddress("sender@domain.com");
            testMail.Subject = "I am subject";
            testMail.Body = "<head>Jason Lee</head>";
            testMail.IsBodyHtml = true;

            testMail.Save(@"c:\Temp\html.eml");

        }

        [Fact]
        public void Dundee()
        {
            MailMessage testMail = new MailMessage();
            testMail.To.Add(new MailAddress("jawc@domain.com"));
            testMail.From = new MailAddress("sender@domain.com");
            testMail.Subject = "I am subject";


            String htmlBody = "郵件";
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

            testMail.AlternateViews.Add(htmlView);
            testMail.Save(@"c:\Temp\jason.eml");
        }

        [Fact]
        public void SaveHtmlWithImage()
        {
            MailMessage testMail = new MailMessage();
            testMail.To.Add(new MailAddress("jawc@domain.com"));
            testMail.From = new MailAddress("sender@domain.com");
            testMail.Subject = "I am subject";

            String htmlBody = "<img src=\"cid:uniqueId\" />";
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

            LinkedResource image = new LinkedResource(@"c:\temp\Ascent.jpg");
            image.ContentId = "uniqueId";
            htmlView.LinkedResources.Add(image);

            testMail.AlternateViews.Add(htmlView);
            testMail.Save(@"c:\Temp\testemail.eml");

        }

        [Fact]
        public void Load()
        {
            FileInfo file = new FileInfo(@"c:\Temp\dundee.eml");
            MailMessage testMail = MailMessageMimeParser.ParseMessage(file.FullName);

            //ContentType bb = testMail.AlternateViews[0].LinkedResources[0].ContentType;
            //testMail.AlternateViews[0].LinkedResources[0].ContentType = bb;
            testMail.Save(@"c:\Temp\dundee1.eml");
        }

        [Fact]
        public void SendMail()
        {
            FileInfo file = new FileInfo(@"c:\Temp\Office2007.eml");
            MailMessage testMail = MailMessageMimeParser.ParseMessage(file.FullName);

            SmtpClient smtpClient = new SmtpClient("172.17.16.222", 25);
            //smtpClient.Credentials = new System.Net.NetworkCredential("uguardtest", "S2nACfCFL.1lvd");
            smtpClient.Send(testMail);

        }
    }
}
