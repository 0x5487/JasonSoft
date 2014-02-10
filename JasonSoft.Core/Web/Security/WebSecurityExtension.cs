using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using JasonSoft.XML;

namespace JasonSoft.Web.Security
{
    public static class WebSecurityExtension
    {
        /// <summary>
        /// Encodes a cookie with all protection levels
        /// </summary>
        /// <param name="cookie">The cookie to encode</param>
        /// <returns>A clone of the cookie in encoded format</returns>
        public static HttpCookie Encode(this HttpCookie cookie)
        {
            return Encode(cookie, CookieProtection.All);
        }

        /// <summary>
        /// Encodes a cookie
        /// </summary>
        /// <param name="cookie">The cookie to encode</param>
        /// <param name="cookieProtection">The cookie protection to set</param>
        /// <returns>A clone of the cookie in encoded format</returns>
        public static HttpCookie Encode(this HttpCookie cookie, CookieProtection cookieProtection)
        {
            HttpCookie encodedCookie = CloneCookie(cookie);
            encodedCookie.Value = MachineKeyCryptography.Encode(cookie.Value, cookieProtection);
            return encodedCookie;
        }

        /// <summary>
        /// Decodes a cookie that has all levels of cookie protection. Throws InvalidCypherTextException if unable to decode.
        /// </summary>
        /// <param name="cookie">The cookie to decode</param>
        /// <returns>A clone of the cookie in decoded format</returns>
        public static HttpCookie Decode(this HttpCookie cookie)
        {
            return Decode(cookie, CookieProtection.All);
        }

        /// <summary>
        /// Decodes a cookie. Throws InvalidCypherTextException if unable to decode.
        /// </summary>
        /// <param name="cookie">The cookie to decode</param>
        /// <param name="cookieProtection">The protection level to use when decoding</param>
        /// <returns>A clone of the cookie in decoded format</returns>
        public static HttpCookie Decode(this HttpCookie cookie, CookieProtection cookieProtection)
        {
            HttpCookie decodedCookie = CloneCookie(cookie);
            decodedCookie.Value = MachineKeyCryptography.Decode(cookie.Value, cookieProtection);
            return decodedCookie;
        }

        /// <summary>
        /// Creates a clone of the given cookie
        /// </summary>
        /// <param name="cookie">A cookie to clone</param>
        /// <returns>The cloned cookie</returns>
        private static HttpCookie CloneCookie(HttpCookie cookie)
        {
            HttpCookie clonedCookie = new HttpCookie(cookie.Name, cookie.Value);
            clonedCookie.Domain = cookie.Domain;
            clonedCookie.Expires = cookie.Expires;
            clonedCookie.HttpOnly = cookie.HttpOnly;
            clonedCookie.Path = cookie.Path;
            clonedCookie.Secure = cookie.Secure;

            return clonedCookie;
        }

        public static String RemoveScript(this String source)
        {
            String content = "<JasonSoft>" + source + "</JasonSoft>";
            content = content.ToXHTML();

            XDocument xhtml = XDocument.Parse(content);
            var results = from tr in xhtml.Elements("JasonSoft").Elements() select tr;

            foreach (XElement result in results)
            {
                RemoveScriptEvent(result);
            }

            content = xhtml.ToString();

            //remove <script> tags
            Int32 intStartIndex = content.ToLower().IndexOf("<script");
            while (intStartIndex >= 0)
            {
                content = content.Remove(intStartIndex, 8);
                intStartIndex = content.ToLower().IndexOf("<script");
            }
            intStartIndex = content.ToLower().IndexOf("</script>");
            while (intStartIndex >= 0)
            {
                content = content.Remove(intStartIndex, 9);
                intStartIndex = content.ToLower().IndexOf("</script>");
            }

            //remove <JasonSoft> tag
            content = content.Clip(11);
            content = content.Chop(12);
            return content;
        }

        private static void RemoveScriptEvent(XElement element)
        {
            if (element.Name.ToString().ToLower() == "script")
            {
                //remove element will cause foreach to stop.  Therefore, don't remove it, but set the value to empty.
                element.Value = String.Empty;
                return;
            }

            if(element.Name.ToString().ToLower() == "a")
            {
                XAttribute link = element.Attribute("href");
                if(link != null)
                {
                    if(link.Value.ToLower().IndexOf("javascript") == 0)
                    {
                        link.Value = "#";
                    }
                }
            }

            //remove event
            foreach (XAttribute attribute in element.Attributes())
            {
                switch (attribute.Name.ToString().ToLower())
                {
                    case "onblur":
                    case "onclick":
                    case "ondbclick":
                    case "ondatabinding":
                    case "ondisposed":
                    case "onfocus":
                    case "oninit":
                    case "onkeydown":
                    case "onkeypress":
                    case "onkeyup":
                    case "onload":
                    case "onmousedown":
                    case "onmousemove":
                    case "onmouseout":
                    case "onmouseover":
                    case "onmouseup":
                    case "onprerender":
                    case "onserverclick":
                    case "onunload":
                        attribute.Remove();
                        break;
                }
            }


            foreach (XElement xElement in element.Elements())
            {
                RemoveScriptEvent(xElement);
            }
        }
    }
}
