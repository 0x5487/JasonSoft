// Copyright 2008 JasonSoft - http://www.jasonsoft.net/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Configuration;
using System.Web;
using JasonSoft.Components.Castle.MicroKernel.Lifestyle;
using JasonSoft.Components.Container;

namespace JasonSoft.Components.Castle.MicroKernel.Lifestyle
{
    /// <summary>
    /// Implements a Lifestyle Manager for Web Apps that
    /// create at most one object per web request.
    /// </summary>
    [Serializable]
    public class PerWebRequestLifestyleManager : AbstractLifestyleManager
    {
        private string PerRequestObjectID = "PerRequestLifestyleManager_" + Guid.NewGuid().ToString();

        #region ILifestyleManager Members

        public override object Resolve(CreationContext context)
        {
            HttpContext current = HttpContext.Current;

            if (current == null)
                throw new InvalidOperationException(
                    "HttpContext.Current is null.  WebRequestInstanceScopeModule can only be used in ASP.Net");

            if (current.Items[PerRequestObjectID] == null)
            {
                if (!WebRequestInstanceScopeModule.Initialized)
                {
                    string message = "Looks like you forgot to register the http module " +
                                     typeof (WebRequestInstanceScopeModule).FullName +
                                     "\r\nAdd '<add name=\"PerRequestLifestyle\" type=\"JasonSoft.Components.Container.WebRequestInstanceScopeModule, JasonSoft.Core\" />' " +
                                     "to the <httpModules> section on your web.config";

                    throw new ConfigurationErrorsException(message);
                }

                object instance = base.Resolve(context);
                current.Items[PerRequestObjectID] = instance;
                WebRequestInstanceScopeModule.RegisterForEviction(this, instance);
            }

            return current.Items[PerRequestObjectID];
        }

        public override void Release(object instance)
        {
            // Since this method is called by the kernel when an external
            // request to release the component is made, it must do nothing
            // to ensure the component is available during the duration of 
            // the web request.  An internal Evict method is provided to
            // allow the actual releasing of the component at the end of
            // the web request.
        }

        internal void Evict(object instance)
        {
            base.Release(instance);
        }

        public override void Dispose()
        {
        }

        #endregion
    }
}

namespace JasonSoft.Components.Container
{
    public class WebRequestInstanceScopeModule : IHttpModule
    {
        private static bool initialized;

        private const string PerRequestEvict = "PerRequestLifestyleManager_Evict";

        public void Init(HttpApplication context)
        {
            initialized = true;
            context.EndRequest += new EventHandler(Application_EndRequest);
        }

        public void Dispose()
        {
        }

        internal static void RegisterForEviction(PerWebRequestLifestyleManager manager, object instance)
        {
            HttpContext context = HttpContext.Current;

            IDictionary candidates = (IDictionary) context.Items[PerRequestEvict];

            if (candidates == null)
            {
                candidates = new Hashtable();
                context.Items[PerRequestEvict] = candidates;
            }

            candidates.Add(manager, instance);
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication) sender;
            IDictionary candidates = (IDictionary) application.Context.Items[PerRequestEvict];

            if (candidates != null)
            {
                foreach (DictionaryEntry candidate in candidates)
                {
                    PerWebRequestLifestyleManager manager =
                        (PerWebRequestLifestyleManager) candidate.Key;
                    manager.Evict(candidate.Value);
                }

                application.Context.Items.Remove(PerRequestEvict);
            }
        }

        internal static bool Initialized
        {
            get { return initialized; }
        }
    }
}