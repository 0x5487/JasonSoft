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

using System.Collections.Generic;

namespace JasonSoft.Components.Quartz.Collection
{
	/// <summary>
	/// Represents a collection ob objects that contains no duplicate elements.
	/// </summary>	
	/// <author>Marko Lahma (.NET)</author>
	public interface ISet<T> : IList<T>
	{
		/// <summary>
		/// Adds all the elements of the specified collection to the Set.
		/// </summary>
		/// <param name="c">Collection of objects to add.</param>
		/// <returns>true</returns>
		bool AddAll(ICollection<T> c);

		/// <summary>
		/// Returns the first item in the set.
		/// </summary>
		/// <returns>First object.</returns>
		T First();
	}
}