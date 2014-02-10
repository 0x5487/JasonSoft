using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using JasonSoft.Net;
using JasonSoft.XML;
using Xunit;


namespace JasonSoft.Tests.Net
{

    public class WebDownloaderTestCase
    {
        [Fact]
        public void MultithreadDownloadTest()
        {
            WebDownloader downloader = new WebDownloader();

            for (Int32 i = 202060; i < 202080; i++)
            {
                downloader.Queue.Enqueue(new WebDownloader.InputParameter()
                                             {
                                                 Url = string.Format(@"http://rent.591.com.tw/rent-detail-{0}.html", i),
                                                 SaveAs = @"C:\WebPage\591\" + i.ToString() + ".html"
                                             });
            }

            List<WebDownloader.OutParameter> results = downloader.StartAndWaitForIdle(12);
            downloader.Stop();
            Assert.Same(12, results.Count);
        }

        [Fact]
        public void DownloadRecovery()
        {
            WebDownloader downloader = new WebDownloader();

            for (Int32 i = 31250; i < 31280; i++)
            {
                downloader.Queue.Enqueue(new WebDownloader.InputParameter()
                {
                    Url = string.Format(@"http://www.nessus.org/plugins/index.php?view=single&id={0}", i),
                    SaveAs = @"C:\WebPage\Nessus\" + i.ToString() + ".html"
                });
            }

            String dump = downloader.Queue.ToList().ToXML();
            Console.WriteLine(dump);

            List<WebDownloader.InputParameter> toDoList = dump.FromXML<List<WebDownloader.InputParameter>>();
            
            downloader = new WebDownloader();

            foreach (var item in toDoList)
            {
                downloader.Queue.Enqueue(item);
            }

            List<WebDownloader.OutParameter> results = downloader.StartAndWaitForIdle(12);
            downloader.Stop();
            Assert.Same(12, results.Count);
        }

        [Fact]
        public void SingleDowload()
        {
            String url = "http://www.juniper.net/security/auto/vulnerabilities/vuln3492.html";

            //WebClient brower = new WebClient();
            //brower.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //brower.Headers.Add("Accept-Language", "zh-tw");
            //brower.DownloadFile(url, @"C:\WebPage\Juniper\1.html");

            WebDownloader downloader = new WebDownloader();

            for (Int32 i = 0; i < 30; i++ )
            {
                downloader.Queue.Enqueue(new WebDownloader.InputParameter() { Url = url, SaveAs = @"C:\WebPage\Juniper\" + i.ToString() + ".html" });

            }

            
            downloader.StartAndWaitForIdle();
            downloader.Stop();
        }
    }
}
