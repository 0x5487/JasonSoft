using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace JasonSoft.Web.Routing
{
    public static class WebFormRouteExtensions
    {
        public static void MapWebFormRoute(this RouteCollection routes, string url, string virtualPath) 
        {
            routes.Add(null, new WebFormRoute(url, virtualPath));
        }

        public static void MapWebFormRoute(this RouteCollection routes, string name, string url, string virtualPath) 
        {
            routes.Add(name, new WebFormRoute(url, virtualPath));
        }

        public static void MapWebFormRoute(this RouteCollection routes, string name, string url,  string virtualPath, RouteValueDictionary defaults)
        {
            routes.Add(name, new WebFormRoute(url, virtualPath) {Defaults = defaults});
        }

        public static void MapWebFormRoute(this RouteCollection routes, string name, string url, string virtualPath, RouteValueDictionary defaults, RouteValueDictionary constraints)
        {
            routes.Add(name, new WebFormRoute(url, virtualPath) {Constraints = constraints, Defaults = defaults});
        }

        public static void MapWebFormRoute(this RouteCollection routes, string url, string virtualPath, bool checkPhysicalUrlAccess) 
        {
            routes.Add(null, new WebFormRoute(url, virtualPath, checkPhysicalUrlAccess));
        }

        public static void MapWebFormRoute(this RouteCollection routes, string name, string url, string virtualPath, bool checkPhysicalUrlAccess)
        {
            routes.Add(name, new WebFormRoute(url, virtualPath, checkPhysicalUrlAccess));
        }

        public static void MapWebFormRoute(this RouteCollection routes, string name, string url, string virtualPath, bool checkPhysicalUrlAccess, RouteValueDictionary constraints)
        {
            routes.Add(name, new WebFormRoute(url, virtualPath, checkPhysicalUrlAccess) { Constraints = constraints});
        }
    }
}
