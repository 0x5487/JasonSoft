using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JasonSoft.Web
{
    public static partial class WebHelper
    {

        public static String GetValidateFailedMessage(Page source)
        {
            StringBuilder message = new StringBuilder();

            ValidatorCollection oValidatorCollection = source.Validators;
            if (oValidatorCollection.IsNullOrEmpty()) return null;

            foreach (BaseValidator oValidator in oValidatorCollection)
            {
                if (!oValidator.IsValid)
                {
                    string sErrorMessage = oValidator.ErrorMessage + @"\n";
                    message.Append(sErrorMessage);
                }
            }

            return message.ToString();
        }

        public static string ResolveHttpUrl(string relativeUrl)
        {
            Control control = new Control();
            return GetHostUrl() + control.ResolveUrl(relativeUrl);
        }

        public static string ResolveUrl(string relativeUrl)
        {
            Control control = new Control();
            return control.ResolveUrl(relativeUrl);
        }

        public static String GetWebSiteUrl()
        {
            return GetHostUrl() + HttpContext.Current.Request.ApplicationPath;
        }

        public static string GetHostUrl()
        {
            string securePort = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            string protocol = securePort == null || securePort == "0" ? "http" : "https";
            string serverPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            string port = serverPort == "80" ? string.Empty : ":" + serverPort;
            string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            return string.Format("{0}://{1}{2}", protocol, serverName, port);
        }

        public static T QueryString<T>(String name)
        {
            Type targetType = typeof(T);
            String result = HttpContext.Current.Request.QueryString[name];

            if (targetType.IsNullableType() == false && result == null)
            {
                throw new HttpException(404, "convert type error from webhelper");
            }

            return result.ChangeTypeTo<T>();
        }

        public static T Post<T>(String name)
        {
            Type targetType = typeof(T);
            String result = HttpContext.Current.Request.Form[name];

            if (targetType.IsNullableType() == false && result == null)
            {
                throw new HttpException(404, "convert type error from webhelper");
            }

            return result.ChangeTypeTo<T>();
        }

        

    }
}
