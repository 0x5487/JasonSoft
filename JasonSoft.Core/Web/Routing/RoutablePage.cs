using System;
using System.Web.UI;
using System.Web.Routing;
using System.Web;

namespace JasonSoft.Web.Routing
{
    /// <summary>
    /// Handy base class for routable pages.
    /// </summary>
    public abstract class RoutablePage : Page, IRoutablePage
    {
        public RequestContext RequestContext {get; set; }

        public RouteData RouteData 
        {
            get 
            {
                if (RequestContext == null)
                {
                    //Try to manafacture one.
                    var context = new HttpContextWrapper(HttpContext.Current);
                    var requestContext = new RequestContext(context, new RouteData());
                    this.RequestContext = requestContext;
                }
                
                return this.RequestContext.RouteData;
            }
        }

        private string RouteUrl(string name, RouteValueDictionary values)
        {
            
            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(RequestContext, name, values);
            if (vpd != null)
            {
                return vpd.VirtualPath;
            }
            return null;
        }

        public String GetRouteUrl(String name, Object values)
        {
            return RouteUrl(name, new RouteValueDictionary(values));
        }



    }
}
