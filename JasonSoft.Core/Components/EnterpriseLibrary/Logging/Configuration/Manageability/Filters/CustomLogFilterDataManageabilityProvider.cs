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
using JasonSoft.Components.EnterpriseLibrary.Common.Configuration.Manageability;
using JasonSoft.Components.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;

namespace JasonSoft.Components.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    /// <summary>
    /// Provides an implementation for <see cref="CustomLogFilterData"/> that
    /// processes policy overrides, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class CustomLogFilterDataManageabilityProvider
        : CustomProviderDataManageabilityProvider<CustomLogFilterData>
    {
        /// <summary>
        /// The name of the attributes property.
        /// </summary>
        public new const String AttributesPropertyName = CustomProviderDataManageabilityProvider<CustomLogFilterData>.AttributesPropertyName;

        /// <summary>
        /// The name of the type property.
        /// </summary>
        public const String TypePropertyName = ProviderTypePropertyName;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomLogFilterDataManageabilityProvider"/> class
        /// </summary>
        public CustomLogFilterDataManageabilityProvider()
            : base(Resources.FilterPolicyNameTemplate)
        { }
    }
}
