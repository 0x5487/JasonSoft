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
using System.Net;

#endregion

namespace JasonSoft.Web.Compression
{
    public abstract class CompressionHandlerBase : IHttpAsyncHandler
    {

        #region IHttpAsyncHandler Members

        private Action<HttpContext> _AsyncProcessRequest;

        /// <summary>
        /// Begin the async process
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cb"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            _AsyncProcessRequest = new Action<HttpContext>(ProcessRequest);
            return _AsyncProcessRequest.BeginInvoke(context, cb, extraData);
        }
        /// <summary>
        /// End the async process
        /// </summary>
        /// <param name="result"></param>
        public void EndProcessRequest(IAsyncResult result)
        {
            _AsyncProcessRequest.EndInvoke(result);
        }

        #endregion


        #region IHttpHandler Members

        /// <summary>
        /// Process the request
        /// </summary>
        /// <param name="context"></param>
        virtual public void ProcessRequest(HttpContext context)
        {
            int versionHash;
            DateTime lastUpdate;
            string[] relativeFiles = context.Request.QueryString["d"].Split(',');
            string[] absoluteFiles = GetFilesInfo(relativeFiles,context, out versionHash,out lastUpdate);

            context.Response.Clear();
            SetHeaders(context, lastUpdate, versionHash);

            CheckETag(context, versionHash);

            Minifier currentMinifier = new Minifier(Minify);
            EncodingManager encodingMgr = new EncodingManager(context);
            if (!IsCompressContent())
            {
                encodingMgr.PreferredEncodingType = EncodingManager.EncodingType.None;
            }
            Settings.Instance.CurrentStorage.Excute(context, absoluteFiles, currentMinifier, versionHash, encodingMgr);
        }


        /// <summary>
        /// 
        /// </summary>
        virtual public bool IsReusable
        {
            get { return true; }
        }
        #endregion


        #region // Methods

        /// <summary>
        /// Check if the ETag that sent from the client is match to the current ETag.
        /// If so, set the status code to 'Not Modified' and stop the response.
        /// </summary>
        private static void CheckETag(HttpContext context, int etagCode)
        {
            string etag = string.Concat("\"", etagCode, "\"");
            string incomingEtag = context.Request.Headers["If-None-Match"];

            if (incomingEtag != null && etag.Equals(incomingEtag, StringComparison.Ordinal))
            {
                context.Response.Clear();
                context.Response.AppendHeader("Content-Length", "0");
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                context.Response.SuppressContent = true;
                context.Response.End();
            }
        }
        /// <summary>
        /// Get array of files info, and the hash code of the total files
        /// </summary>
        /// <param name="relativeFiles"></param>
        /// <param name="context"></param>
        /// <param name="hashCode"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        private string[] GetFilesInfo(string[] relativeFiles,HttpContext context, out int hashCode,out DateTime lastUpdate)
        {
            DateTime minDate = DateTime.UtcNow;
            int hash = 0;
            string currentPath = Util.GetCurrentPath(context);
            string[] files = new string[relativeFiles.Length];

            for (int i = 0; i < relativeFiles.Length; i++)
            {
                string file = relativeFiles[i];

                VerifyPermission(file);

                string absoluteFile;
                if (relativeFiles[i][0].Equals('~'))
                {
                    absoluteFile = context.Server.MapPath(relativeFiles[i]);
                }
                else
                {
                    absoluteFile = context.Server.MapPath(currentPath + relativeFiles[i]);
                }
                FileInfo fi = new FileInfo(absoluteFile);
                if (fi.Exists)
                {
                    DateTime utcModified = fi.LastWriteTime.ToUniversalTime();
                    if (utcModified < minDate)
                    {
                        minDate = utcModified;
                    }

                    hash = Util.CombineHashCodes(utcModified.GetHashCode() + fi.FullName.GetHashCode(), hash);
                    files[i] = absoluteFile;
                }
                else
                {
                    files[i] = "NOT_FOUND|" + file;
                }
            }
            hashCode = hash;
            lastUpdate = minDate;
            return files;
        }

        /// <summary>
        /// Set the headers for the response
        /// </summary>
        /// <param name="context"></param>
        /// <param name="lastUpdate"></param>
        /// <param name="versionHash"></param>
        private static void SetHeaders(HttpContext context, DateTime lastUpdate, int versionHash)
        {
           //if (lastUpdate > DateTime.UtcNow)
           // {
           //     lastUpdate = DateTime.UtcNow.AddMinutes(-1);
           // }
            context.Response.Cache.VaryByParams["d"] = true;
            context.Response.Cache.VaryByParams["v"] = true;
            context.Response.Cache.SetOmitVaryStar(true);
            context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;

            // Keep in the client cache for the time configured in the Web.Config
            context.Response.Cache.SetExpires(DateTime.Now.ToUniversalTime().AddDays(Settings.Instance.DaysInCache));
            context.Response.Cache.SetMaxAge(TimeSpan.FromDays(Settings.Instance.DaysInCache));
            context.Response.Cache.SetValidUntilExpires(true);
            // context.Response.Cache.SetLastModified(lastUpdate);

            context.Response.Cache.SetCacheability(HttpCacheability.Public);

            context.Response.AppendHeader("ETag", string.Concat("\"", versionHash, "\""));
        }

   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        virtual protected string Minify(StreamReader reader)
        {
            return reader.ReadToEnd();
        }
        

        /// <summary>
        /// Verify that the handled file is permitted
        /// </summary>
        /// <remarks>Force the inherited calss to implement the VeriyPermission method</remarks>
        /// <param name="file"></param>
        protected abstract void VerifyPermission(string file);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsCompressContent();

        #endregion

    }
}
