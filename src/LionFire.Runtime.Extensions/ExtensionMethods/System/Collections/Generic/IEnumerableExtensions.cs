﻿using System.Collections.Generic;

namespace LionFire.ExtensionMethods
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Wraps this object instance into an IEnumerable&lt;T&gt;
        /// consisting of a single item.
        /// </summary>
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="item"> The instance that will be wrapped. </param>
        /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
        public static IEnumerable<T> Yield<T>(this T item) // RENAME to ToEnumerable?
        {
            yield return item;
        }
    }
}