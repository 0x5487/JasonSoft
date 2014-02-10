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
using JasonSoft.Components.EnterpriseLibrary.Common.Configuration;
using JasonSoft.Components.EnterpriseLibrary.Common.Configuration.ContainerModel;
using JasonSoft.Components.EnterpriseLibrary.Logging.Formatters;
using JasonSoft.Components.EnterpriseLibrary.Logging.Properties;
using System.Collections.Generic;

namespace JasonSoft.Components.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents the configuration settings for a log formatter.  This class is abstract.
	/// </summary>
	public class FormatterData : NameTypeConfigurationElement
	{
		/// <summary>
		/// Create a new instance of a <see cref="FormatterData"/>.
		/// </summary>
		public FormatterData()
		{
		}

		/// <summary>
		/// Create a new instance of a <see cref="FormatterData"/>.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="formatterType">The <see cref="ILogFormatter"/> type.</param>
		public FormatterData(string name, Type formatterType)
			: base(name, formatterType)
		{
		}

	    /// <summary>
	    /// Returns the <see cref="TypeRegistration"/> entry for this data section.
	    /// </summary>
	    /// <returns>The type registration for this data section</returns>
	    public virtual IEnumerable<TypeRegistration> GetRegistrations()
	    {
            throw new NotImplementedException(Resources.ExceptionMethodMustBeImplementedBySubclasses);
	    }
	}
}
