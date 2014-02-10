//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using JasonSoft.Components.EnterpriseLibrary.Common.Configuration.Manageability;
using JasonSoft.Components.EnterpriseLibrary.Common.Instrumentation.Configuration;

[assembly : ConfigurationSectionManageabilityProvider(InstrumentationConfigurationSection.SectionName, typeof(InstrumentationConfigurationSectionManageabilityProvider))]
