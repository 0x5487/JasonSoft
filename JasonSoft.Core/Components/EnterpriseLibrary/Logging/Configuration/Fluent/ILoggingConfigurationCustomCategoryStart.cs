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

namespace JasonSoft.Components.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface that allows settings to be configured for a custom category source.
    /// </summary>
    public interface ILoggingConfigurationCustomCategoryStart : ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Returns a fluent interface that can be used to further configure a custom category source.
        /// </summary>
        ILoggingConfigurationCustomCategoryOptions WithOptions
        {
            get;
        }
    }
}
