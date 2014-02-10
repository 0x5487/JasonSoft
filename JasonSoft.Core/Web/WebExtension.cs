using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using JasonSoft;
using JasonSoft.Web.Security.AntiXss;


namespace JasonSoft.Web
{
    public static class WebExtension
    {
        public static NameValueCollection ToNameValueCollectio(this String source)
        {
            String[] pair = source.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            if (pair.IsNullOrEmpty()) return null;

            NameValueCollection results = new NameValueCollection();

            foreach (string s in pair)
            {
                String[] result = s.Split('=');
                results.Add(result[0], result[1]);
            }

            return results;
        }


        public static void RedirectPermanent(this HttpResponse response, String pathOrUrl)
        {
            response.Clear();
            response.Status = "301 Moved Permanently";
            response.RedirectLocation = pathOrUrl;
            response.End();
        }

        public static void AddMetadata(this Page source, String name, String content)
        {
            if(name.IsNullOrEmpty()) throw new ArgumentException("can't be empty or null", name);
            if(content.IsNullOrEmpty()) throw new ArgumentException("can't be empty or null", content);

            HtmlMeta meta = new HtmlMeta();
            meta.Name = name;
            meta.Content = content;
            source.Controls.Add(meta);
        }

        public static string UrlEncrypt(this string source)
        {
            if(source.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("source");
            }

            string url = EncryptHelper.Encrypt(source);
            return AntiXss.UrlEncode(url);
        }

        public static string UrlDecrypt(this string source)
        {
            if (source.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("source");
            }

            string url = HttpUtility.UrlDecode(source);
            return EncryptHelper.Decrypt(url);
        }
    }
}
