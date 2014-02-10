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
using System.Collections.ObjectModel;

namespace JasonSoft.Components.Quartz.Collection
{
    /// <summary>
    /// SupportClass for the TreeSet class.
    /// </summary>
    /// <author>Marko Lahma (.NET)</author>
    [Serializable]
    public class TreeSet<T> : List<T>, ISortedSet<T>
    {
        private readonly IComparer<T> comparator = Comparer<T>.Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeSet&lt;T&gt;"/> class.
        /// </summary>
        public TreeSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeSet&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        public TreeSet(ICollection<T> c)
        {
            AddAll(c);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeSet&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        public TreeSet(IComparer<T> c)
        {
            comparator = c;
        }

        /// <summary>
        /// Unmodifiables the tree set.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static TreeSet<T> UnmodifiableTreeSet(ICollection<T> collection)
        {
            ReadOnlyCollection<T> items = new List<T>(collection).AsReadOnly();
            return new TreeSet<T>(items);
        }

        /// <summary>
        /// Gets the IComparator object used to sort this set.
        /// </summary>
        public IComparer<T> Comparator
        {
            get { return comparator; }
        }

        private bool AddWithoutSorting(T obj)
        {
            bool inserted;
            if (!(inserted = Contains(obj)))
            {
                base.Add(obj);
            }
            return !inserted;
        }

        /// <summary>
        /// Adds a new element to the ArrayList if it is not already present and sorts the ArrayList.
        /// </summary>
        /// <param name="obj">Element to insert to the ArrayList.</param>
        /// <returns>TRUE if the new element was inserted, FALSE otherwise.</returns>
        public new bool Add(T obj)
        {
            bool inserted = AddWithoutSorting(obj);
            Sort(comparator);
            return inserted;
        }

        /// <summary>
        /// Adds all the elements of the specified collection that are not present to the list.
        /// </summary>		
        /// <param name="c">Collection where the new elements will be added</param>
        /// <returns>Returns true if at least one element was added to the collection.</returns>
        public bool AddAll(ICollection<T> c)
        {
            IEnumerator<T> e = new List<T>(c).GetEnumerator();
            bool added = false;
            while (e.MoveNext())
            {
                if (AddWithoutSorting(e.Current))
                {
                    added = true;
                }
            }
            Sort(comparator);
            return added;
        }

        /// <summary>
        /// Returns the first item in the set.
        /// </summary>
        /// <returns>First object.</returns>
        public T First()
        {
            return this[0];
        }

        /// <summary>
        /// Returns a portion of the list whose elements are greater than the limit object parameter.
        /// </summary>
        /// <param name="limit">The start element of the portion to extract.</param>
        /// <returns>The portion of the collection whose elements are greater than the limit object parameter.</returns>
        public ISortedSet<T> TailSet(T limit)
        {
            ISortedSet<T> newList = new TreeSet<T>();
            int i = 0;
            while ((i < Count) && (comparator.Compare(this[i], limit) < 0))
            {
                i++;
            }
            for (; i < Count; i++)
            {
                newList.Add(this[i]);
            }
            return newList;
        }
    }
}