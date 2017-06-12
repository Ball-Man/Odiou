using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odiou
{
    /// <summary>
    /// Implementation of a dictionary similar to the .net standard one, but this time it's bidirectional. Blame on you, microsoft
    /// It's not actually sorted, but acts like it was. New added elements are always placed respecting the order, but the key vector given to the constructor has to be previously sorted
    /// </summary>
    /// <typeparam name="TKey">Type used for keys</typeparam>
    /// <typeparam name="TValue">Type used for values</typeparam>
    public class SortedBiDictionary<TKey, TValue>
    {
        /// <summary>
        /// Class field used for sortings and binary searches
        /// </summary>
        private IComparer<TKey> _comparer;

        /// <summary>
        /// The list of keys
        /// </summary>
        public List<TKey> Keys { get; private set; } = new List<TKey>();

        /// <summary>
        /// The list of values
        /// </summary>
        public List<TValue> Values { get; private set; } = new List<TValue>();

        /// <summary>
        /// Gets or sets the value corresponding to the given key
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                int index = _comparer != null ? Keys.BinarySearch(key, _comparer) : Keys.BinarySearch(key);
                if (index >= 0)
                    return Values[index];
                return default(TValue);
            }

            set
            {
                int index = _comparer != null ? Keys.BinarySearch(key, _comparer) : Keys.BinarySearch(key);
                if (index >= 0)
                    Values[index] = value;
                else
                {
                    int bIndex = ~index;
                    Keys.Insert(bIndex, key);
                    Values.Insert(bIndex, value);
                }
            }
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        public SortedBiDictionary() { }

        /// <summary>
        /// Creates a bidirectional dictionary. Requires a sorted keys array
        /// </summary>
        /// <param name="keys">The vector of keys</param>
        /// <param name="values">The vector of values</param>
        public SortedBiDictionary(IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            if (keys.Count() != values.Count())
                throw new FormatException("Can't pair two vectors of different lengths");

            Keys = new List<TKey>(keys);
            Values = new List<TValue>(values);
        }

        /// <summary>
        /// Creates a bidirectional dictionary based on the given order. Requires a sorted keys array
        /// </summary>
        /// <param name="keys">The vector of keys</param>
        /// <param name="values">The vector of values</param>
        /// <param name="comparer">The comparer used for binary searches</param>
        public SortedBiDictionary(IEnumerable<TKey> keys, IEnumerable<TValue> values, IComparer<TKey> comparer) : this(keys, values)
        {
            _comparer = comparer;
        }

        /// <summary>
        /// Gets the key corresponding to the first occurence of the given value. If no key was found throws an exception
        /// </summary>
        /// <param name="value">The value used for the look up</param>
        /// <returns>The key value</returns>
        public TKey KeyLookUp(TValue value, Delegate method)
        {
            int index = Values.IndexOf(value);
            if (index >= 0)
                return Keys[index];
            throw new KeyNotFoundException("No key corresponding to the given value");
        }

        /// <summary>
        /// interprits the given key as the nearest existing one and returns the corresponding value
        /// </summary>
        /// <param name="key">The given key</param>
        public TValue GetNearestValue(TKey key)
        {
            int index = _comparer != null ? Keys.BinarySearch(key, _comparer) : Keys.BinarySearch(key);
            if (index >= 0)
                return Values[index];
            return Values[~index];
        }
    }
}
