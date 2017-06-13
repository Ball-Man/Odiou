using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odiou
{
    /// <summary>
    /// Used to determine the difference between two items(similar to IComparer/Comparable)
    /// </summary>
    public interface IDifferencer<T> : IComparer<T>
    {
        /// <summary>
        /// Gets an integer representing the difference between two items(Similar to IComparable, but gives a significant value)
        /// </summary>
        int Difference(T obj, T obj1);
    }
}
