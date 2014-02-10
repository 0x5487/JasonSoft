using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace JasonSoft.Web.Routing
{
    public class WebFormRoute : Route
    {
        public WebFormRoute(string url, string virtualPath)
            : base(url, new WebFormRouteHandler(virtualPath))
        {

        }

        public WebFormRoute(string url, string virtualPath, bool checkPhysicalUrlAccess)
            : base(url, new WebFormRouteHandler(virtualPath, checkPhysicalUrlAccess))
        {

        }



    }
}
