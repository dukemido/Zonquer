using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.Base.Extensions
{
    public static class Extensions
    {
        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (!dict.TryAdd(key, value))
                throw new InvalidOperationException("The operation failed; the key likely exists already.");
        }

        /// <summary>
        ///     Removes a value from a ConcurrentDictionary.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="dict">The dictionary to operate on.</param>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="InvalidOperationException">The key doesn't exist.</exception>
        /// <returns>The value that was removed (if any).</returns>
        public static TValue Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            if (!dict.TryRemove(key, out value))
                throw new InvalidOperationException("The operation failed; the key may not exist.");

            return value;
        }
    }
}
