//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005-VS2008)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 15-08-09
//===========================================================================================


#region Using

using System;
using System.Web;
using System.IO;

#endregion

namespace JasonSoft.Web.Compression
{
    /// <summary>
    /// HttpModule to compress the output of System.Web.UI.Page handler using gzip/deflate algorithm,
    /// corresponding to the client browser support.
    /// <para>
    /// </para>
    /// </summary>
    public sealed class WebCompressionModule : IHttpModule
    {
        Settings settings;

        #region IHttpModule Members

        /// <summary>
        /// Release resources used by the module (Nothing realy in our case), but must implement (interface).
        /// </summary>
        void IHttpModule.Dispose()
        {
            // Nothing to dispose in our case;
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context"></param>
        void IHttpModule.Init(HttpApplication context)
        {
            settings = Settings.Instance;
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
            context.PostRequestHandlerExecute += context_PostRequestHandlerExecute;
            context.PreSendRequestHeaders += context_PreSendRequestHeaders;
            context.EndRequest += context_EndRequest;
        }

        #endregion


        #region  // Page Compression

        /// <summary>
        /// Handles the PreRequestHandlerExecute event.
        /// The compression moved to this step to catch Server.Transfer operation
        /// </summary>
        /// <param name="sender">The object that raised the event (HttpApplication)</param>
        /// <param name="e">The event data</param>
        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            if (app.Context == null || app.Context.Response == null)
                return;

            // This part of the module compress only handlers from type System.Web.UI.Page
            // Other types such JavaScript or CSS files will be compressed in an httpHandelr.
            // Here we check if the current handler if a Page handler, if so, we compress it.
            if (app.Context.CurrentHandler is System.Web.UI.Page && app.Context.Response != null)
            {
                // Check if the path is not excluded.
                if (!settings.IsValidPath(app.Request.AppRelativeCurrentExecutionFilePath))
                    return;

                if (Util.IsToolkitScriptManagerCombiner(app.Request))
                    return;

                if (settings.CompressPage && !(Util.IsMsAjaxRequest(app.Context) && settings.MSAjaxVersion < 3.5))
                {
                    EncodingManager encodingMgr = new EncodingManager(app.Context);

                    if (encodingMgr.IsEncodingEnabled)
                    {
                        // Add those objects to the context.items for use later in the pipeline
                        app.Context.Items.Add("originalFilter", app.Response.Filter);
                        app.Context.Items.Add("preferredEncoding", encodingMgr.PreferredEncoding);
                        //Add the compression filter
                        encodingMgr.CompressResponse();
                    }
                }

                if (settings.AutoMode)
                {
                    bool processCss = settings.CompressCSS || settings.MinifyContent || settings.CombineCSS;
                    bool processJs = settings.CompressJavaScript || settings.MinifyContent;
                    app.Response.Filter = new AutoModeFilterStream(app.Response.Filter, app.Response.ContentEncoding, processJs, processCss, settings.CombineCSS, settings.CombineHeaderScripts);
                }
            }
        }

        /// <summary>
        /// Check here if the current content type is really page content (html/text) and not image or any other
        /// content that is not UI page).
        /// If so, remove the assign compression filter (if was any)
        /// <remarks>
        /// This event is importent when the responde made by page handler (*.aspx) but the MIME type
        /// is not html/text, so we must remove all the previews filters we assigned
        /// </remarks>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            if (app.Context == null || app.Context.Response == null)
                return;

            // The current handler is not really page content (no need to exclude 'not page content' pages)
            if (!Util.IsUIPageContentType(app.Context.Response.ContentType))
            {
                // Remove all the filters added by this module so the content will not be compressed
                Stream originalFilter = app.Context.Items["originalFilter"] as Stream;
                if (originalFilter != null)
                {
                    app.Context.Response.Filter = originalFilter;
                    app.Context.Items.Remove("originalFilter");
                }
                app.Context.Items.Remove("preferredEncoding");
            }
        }

        /// <summary>
        /// Check before sending the headers, if the response marked as compressed, so add the correct "Content-encoding"
        /// header.
        /// <remarks>
        /// This event have two uses:
        /// 1. Add "Content-encoding" header if the responde marked to be compressed
        /// 2. Remove any filter if the MIME type is not html/text, BUT the code have Response.End(), so the 
        ///    event context_PostRequestHandlerExecute will not be fire, and the events order is changed.
        /// Normal order is: 
        /// context_PreRequestHandlerExecute -> 
        /// context_PostRequestHandlerExecute ->
        /// Excute all filters ->
        /// context_PreSendRequestHeaders
        /// 
        /// Order when the code uses Response.End() and usind Response.Flash() :
        /// context_PreRequestHandlerExecute ->
        /// context_PreSendRequestHeaders ->
        /// Excute all filters
        /// </remarks>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            if (app.Context == null || app.Context.Response == null)
                return;

            // The current handler is not really page content.
            if (!Util.IsUIPageContentType(app.Context.Response.ContentType))
            {
                // Remove all the filters added by this module so the content will not be compressed
                Stream originalFilter = app.Context.Items["originalFilter"] as Stream;
                if (originalFilter != null)
                {
                    app.Context.Response.Filter = originalFilter;
                    app.Context.Items.Remove("originalFilter");
                }
                app.Context.Items.Remove("preferredEncoding");
            }
            else
            {
                string preferredEncoding = app.Context.Items["preferredEncoding"] as string;
                if (!string.IsNullOrEmpty(preferredEncoding))
                {
                    app.Context.Response.AppendHeader("Content-encoding", preferredEncoding);
                    app.Context.Items.Remove("preferredEncoding");
                }
            }
        }

        /// <summary>
        /// This event handler hooked only to let redirection from within updatepanel to work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            if (app.Context == null || app.Context.Response == null)
                return;

            if (app.Context.Error != null || (app.Context.Response.IsRequestBeingRedirected && Util.IsMsAjaxRequest(app.Context)))
            {
                // Remove all the filters added by this module so the content will not be compressed
                Stream originalFilter = app.Context.Items["originalFilter"] as Stream;
                if (originalFilter != null)
                {
                    app.Context.Response.Filter = originalFilter;
                    app.Context.Items.Remove("originalFilter");
                }
                app.Context.Items.Remove("preferredEncoding");
            }

        }

        #endregion
    }
}