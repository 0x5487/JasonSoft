using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace JasonSoft.Web.Routing
{

    public static class RouteExtensions
    {
        public static string GetUrlForRoute(this System.Web.UI.Page page, string routeName, RouteValueDictionary parameters)
        {
            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(null, routeName, parameters);
            return vpd.VirtualPath;
        }

        public static string GetUrlForRoute(this System.Web.UI.Page page, RouteValueDictionary parameters)
        {
            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(null, parameters);
            return vpd.VirtualPath;
        }
    }
}
