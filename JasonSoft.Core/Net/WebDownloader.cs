using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using JasonSoft.Components.SmartThreadPool;
using JasonSoft;
using JasonSoft.Math;


namespace JasonSoft.Net
{
    [Serializable]
    public class WebDownloader
    {
        [Serializable]
        public class InputParameter
        {
            public String Url { get; set; }
            public String SaveAs { get; set; }
        }

        [Serializable]
        public class OutParameter
        {
            public Boolean Result { get; set; }
            public Exception Exception { get; set; }
            public InputParameter InputParameter { get; set; }
        }

        private Queue<InputParameter> _queue = null;
        private SmartThreadPool _smartThreadPool = null;
        private Boolean _useBrowerProxySetting = false;
        private List<WebProxy> _proxies = null;
        

        public WebDownloader()
        {
            _queue = new Queue<InputParameter>();
            ThreadCount = 5;
        }

        public Int32 ThreadCount
        {
            get
            {
                return _smartThreadPool.MaxThreads;
            }
            set
            {
                STPStartInfo stpStartInfo = new STPStartInfo();
                stpStartInfo.StartSuspended = true;
                stpStartInfo.MinWorkerThreads = 5;
                stpStartInfo.MaxWorkerThreads = 5;
                _smartThreadPool = new SmartThreadPool(stpStartInfo);
            } 
        }

        public List<WebProxy> Proxies { get; set; }

        public Queue<InputParameter> Queue
        {
            get { return _queue; }
            set { _queue = value;}
        }

        public List<OutParameter> StartAndWaitForIdle()
        {
            return StartAndWaitForIdle(0);
        }

        public List<OutParameter> StartAndWaitForIdle(Int32 limitedCount)
        {
            if(_queue.Count <= 0) return null;

            WorkItemCallback workItem = new WorkItemCallback(Dowload);
            List<IWorkItemResult> workResult = new List<IWorkItemResult>();

            while(_queue.Count > 0)
            {
                if (limitedCount == 0 || workResult.Count < limitedCount)
                    workResult.Add(_smartThreadPool.QueueWorkItem(workItem, _queue.Dequeue()));
                else if (limitedCount >= workResult.Count)
                    break;
            }

            _smartThreadPool.Start();
            _smartThreadPool.WaitForIdle();

            if(workResult.Count > 0)
            {
                List<OutParameter> outputParameter = new List<OutParameter>();

                foreach (var result in workResult)
                {
                    Exception e = null;
                    object obj = result.GetResult(out e);

                    if (e == null)
                    {
                        outputParameter.Add(new OutParameter() {Result = true});
                    }
                    else
                    {
                        // Do something with the exception
                        outputParameter.Add(new OutParameter() {Result = false, Exception = e, InputParameter = (InputParameter)result.State});
                    }
                }

                return outputParameter;
            }

            return null;
        }


        public void Stop()
        {
            _smartThreadPool.Shutdown();
        }


        //To-Do: support binary download and save
        private object Dowload(Object webDownloaderParameter)
        {
            InputParameter inputParameter = webDownloaderParameter as InputParameter;

            //ensure directory is created.
            DirectoryInfo dir = new FileInfo(inputParameter.SaveAs).Directory;
            if (!dir.Exists) dir.Create();

            /* old way coding*/
            //FileInfo file = new FileInfo(inputParameter.SaveAs);
            //StreamWriter sw = null;

            //if (file.Exists)
            //    sw = new StreamWriter(file.OpenWrite(), Encoding.UTF8, 512);
            //else
            //    sw = file.CreateText();
            //WebRequest request = WebRequest.Create(inputParameter.Url);

            //WebResponse response = request.GetResponse();
            //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            //String input = null;
            //while ((input = sr.ReadLine()) != null)
            //{
            //    sw.WriteLine(input);
            //}

            //sw.Flush();
            //sw.Close();
            //response.Close();
            
            WebClient brower = new WebClient();

            if(!_proxies.IsNullOrEmpty())
            {
                if (_proxies.Count == 1) 
                    brower.Proxy = _proxies[0];
                else
                {
                    Int32 order = 0.Random(ThreadCount + 1);
                }

            }

            //brower.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            brower.Headers.Add("Accept-Language", "en-US");
            brower.Headers.Add("Referer", "http://www.591.com.tw");
            brower.Headers.Add("User-Agent", "Mozilla/4.0");
            brower.DownloadFile(inputParameter.Url, inputParameter.SaveAs); //overwrite automaticlly
            
            return true;
        }
    }

}

