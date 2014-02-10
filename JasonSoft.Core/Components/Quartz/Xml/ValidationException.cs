#region License
/* 
 * Copyright 2001-2009 Terracotta, Inc. 
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not 
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at 
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0 
 *   
 * Unless required by applicable law or agreed to in writing, software 
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations 
 * under the License.
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace JasonSoft.Components.Quartz.Xml
{
	/// <summary> 
	/// Reports JobSchedulingDataProcessor validation exceptions.
	/// </summary>
	/// <author> <a href="mailto:bonhamcm@thirdeyeconsulting.com">Chris Bonham</a></author>
    /// <author>Marko Lahma (.NET)</author>
    [Serializable]
	public class ValidationException : Exception
	{
		private readonly ICollection<Exception> validationExceptions = new List<Exception>();

		/// <summary>
		/// Gets the validation exceptions.
		/// </summary>
		/// <value>The validation exceptions.</value>
		public virtual ICollection<Exception> ValidationExceptions
		{
			get { return validationExceptions; }
		}

		/// <summary>
		/// Returns the detail message string.
		/// </summary>
		public override string Message
		{
			get
			{
				if (ValidationExceptions.Count == 0)
				{
					return base.Message;
				}

				StringBuilder sb = new StringBuilder();

				foreach (Exception e in ValidationExceptions)
				{
					sb.AppendLine(e.Message);
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// Constructor for ValidationException.
		/// </summary>
		public ValidationException()    
		{
		}

		/// <summary>
		/// Constructor for ValidationException.
		/// </summary>
		/// <param name="message">exception message.</param>
		public ValidationException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructor for ValidationException.
		/// </summary>
		/// <param name="errors">collection of validation exceptions.</param>
		public ValidationException(IEnumerable<Exception> errors) : this()
		{
			validationExceptions = new List<Exception>(errors).AsReadOnly();
		}
	}
}