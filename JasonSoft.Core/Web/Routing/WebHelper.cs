using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using JasonSoft.Web.Routing;

namespace JasonSoft.Web
{
    public partial class WebHelper
    {
        public static T RouteData<T>(String key)
        {
            Type targetType = typeof(T);

            RoutablePage routablePage = HttpContext.Current.Handler as RoutablePage;
            Object result = routablePage.RouteData.Values[key];

            if(targetType.IsNullableType() == false && result == null)
            {
                throw new HttpException(404, "convert type error from webhelper");
            }

            return result.ChangeTypeTo<T>();

            //if (targetType.IsPrimitive == false && targetType != typeof(String)) throw new ArgumentException();
            //RoutablePage routablePage = HttpContext.Current.Handler as RoutablePage;
            //Object result = routablePage.RouteData.Values[key];
            
            //if (targetType.IsNullableType() == false && result == null)
            //{
            //    throw new HttpException(404, "convert type error from webhelper");
            //}
            //else
            //{
            //    return (T)result.ChangeTypeTo(targetType);
            //}
        }

        public static String RouteUrl(String name)
        {
            if(name.IsNullOrEmpty()) throw new ArgumentNullException("name");

            var context = new HttpContextWrapper(HttpContext.Current);
            var requestContext = new RequestContext(context, new RouteData());

       
            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(requestContext, name, null);

            if (vpd != null)
            {
                return vpd.VirtualPath;
            }
            return null;
        }

        public static string RouteUrl(string name, Object values)
        {
            if(values == null) throw new ArgumentNullException("values");

            var context = new HttpContextWrapper(HttpContext.Current);
            var requestContext = new RequestContext(context, new RouteData());

            RouteValueDictionary myValues = new RouteValueDictionary(values);
            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(requestContext, name, myValues);
            
            if (vpd != null)
            {
                return vpd.VirtualPath;
            }
            return null;
        }

    }
}
