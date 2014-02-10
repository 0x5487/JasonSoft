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
using JasonSoft.Components.EnterpriseLibrary.Common.Configuration.Fluent;
using JasonSoft.Components.EnterpriseLibrary.Common.Properties;
using JasonSoft.Components.EnterpriseLibrary.Common.Properties;

namespace JasonSoft.Components.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="FormatterBuilder"/> extensions to configure <see cref="TextFormatter"/> instances.
    /// </summary>
    /// <seealso cref="TextFormatter"/>
    /// <seealso cref="TextFormatterBuilder"/>
    /// <seealso cref="TextFormatterData"/>
    public static class TextFormatterBuilderExtensions
    {
        /// <summary>
        /// Creates the configuration builder for a <see cref="TextFormatter"/> instance.
        /// </summary>
        /// <param name="builder">Fluent interface extension point.</param>
        /// <param name="formatterName">The name of the <see cref="TextFormatter"/> instance that will be added to configuration.</param>
        /// <seealso cref="TextFormatter"/>
        /// <seealso cref="TextFormatterData"/>
        public static TextFormatterBuilder TextFormatterNamed(this FormatterBuilder builder, string formatterName)
        {
            if (string.IsNullOrEmpty(formatterName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "formatterName");

            return new TextFormatterBuilder(formatterName);
        }
    }
}
