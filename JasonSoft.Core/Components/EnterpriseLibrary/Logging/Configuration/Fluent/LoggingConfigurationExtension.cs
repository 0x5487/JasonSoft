//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JasonSoft.Components.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using System.Messaging;
using JasonSoft.Components.EnterpriseLibrary.Logging.TraceListeners;
using System.Collections.Specialized;
using JasonSoft.Components.EnterpriseLibrary.Logging.Formatters;
using JasonSoft.Components.EnterpriseLibrary.Logging.Properties;
using System.Globalization;

namespace JasonSoft.Components.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary/>
    public abstract class LoggingConfigurationExtension : ILoggingConfigurationOptions, ILoggingConfigurationExtension
    {
        ILoggingConfigurationExtension contextExtension;
        
        /// <summary/>
        protected LoggingConfigurationExtension(ILoggingConfigurationContd context)
        {
            contextExtension = context as ILoggingConfigurationExtension;

            if (contextExtension == null) throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, Resources.ParameterMustImplementType, typeof(ILoggingConfigurationExtension)),
                "context");
        }

        /// <summary/>
        protected LoggingSettings LoggingSettings
        {
            get { return contextExtension.LoggingSettings; }
        }

        /// <summary/>
        protected ILoggingConfigurationOptions LoggingOptions
        {
            get { return contextExtension.LoggingOptions; }
        }


        ILoggingConfigurationOptions ILoggingConfigurationOptions.DisableTracing()
        {
            return LoggingOptions.DisableTracing();
        }

        ILoggingConfigurationOptions ILoggingConfigurationOptions.DoNotRevertImpersonation()
        {
            return LoggingOptions.DoNotRevertImpersonation();
        }

        ILoggingConfigurationOptions ILoggingConfigurationOptions.DoNotLogWarningsWhenNoCategoryExists()
        {
            return LoggingOptions.DoNotLogWarningsWhenNoCategoryExists();
        }

        ILoggingConfigurationCustomCategoryStart ILoggingConfigurationContd.LogToCategoryNamed(string categoryName)
        {
            return LoggingOptions.LogToCategoryNamed(categoryName);
        }

        ILoggingConfigurationSpecialSources ILoggingConfigurationContd.SpecialSources
        {
            get { return LoggingOptions.SpecialSources; }
        }


        ILoggingConfigurationOptions ILoggingConfigurationExtension.LoggingOptions
        {
            get { return contextExtension.LoggingOptions; }
        }

        LoggingSettings ILoggingConfigurationExtension.LoggingSettings
        {
            get { return contextExtension.LoggingSettings ; }
        }
    }
}
