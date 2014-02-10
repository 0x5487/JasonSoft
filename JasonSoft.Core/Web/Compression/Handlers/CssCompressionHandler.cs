//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005-VS2008)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 10-07-08
//===========================================================================================


#region Using

using System;
using System.Web;
using System.IO;

#endregion

namespace JasonSoft.Web.Compression
{
    /// <summary>
    /// 
    /// </summary>
    internal class CssCompressionHandler : CompressionHandlerBase
    {
        private CssCompressionHandler(){}

        public override void ProcessRequest(HttpContext context)
        {
            if (context == null || context.Request == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(context.Request.QueryString["d"]))
            {
                context.Response.ContentType = "text/css";
                base.ProcessRequest(context);
            }
        }

      
        /// <summary>
        ///  Strips the whitespace from any .css file.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override string Minify(StreamReader reader)
        {
            if (Settings.Instance.MinifyContent)
            {
                CssMinifier _Minifier = new CssMinifier();
                return _Minifier.Minify(reader);
            }
            else
            {
                return base.Minify(reader);
            }
        }

        /// <summary>
        /// Verify that the requested file is a javascript file
        /// </summary>
        /// <param name="file"></param>
        protected override void VerifyPermission(string file)
        {
            if (!file.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
            {
                throw new FileLoadException("File type error");
            }
        }

        /// <summary>
        /// Determinate if the current response content should be compressed
        /// </summary>
        /// <returns></returns>
        protected override bool IsCompressContent()
        {
            return Settings.Instance.CompressCSS;
        }
    }
}