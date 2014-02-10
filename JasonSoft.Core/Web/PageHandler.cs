using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace JasonSoft.Web
{
    /// <summary>
    /// Summary description for JasonPageHandler
    /// </summary>
    public class PageHandler : IHttpHandler, IRequiresSessionState
    {
        public PageHandler()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public void ProcessRequest(HttpContext context)
        {
            string rawUrl = context.Request.RawUrl;

            string aspxPagePath = rawUrl.Substring(0, rawUrl.IndexOf(".aspx") + 5);
            IHttpHandler handler = PageParser.GetCompiledPageInstance(aspxPagePath, null, context);

            string aa = handler.GetType().Name;
            string bb = handler.GetType().Assembly.FullName;
            handler.ProcessRequest(context);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}