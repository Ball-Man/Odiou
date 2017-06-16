using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odiou
{
    /// <summary>
    /// Helper class used to get the difference between two ints inside a generic class
    /// </summary>
    class IntDifferencer : IDifferencer<int>
    {
        /// <summary>
        /// Compares two integers
        /// </summary>
        /// <returns>0 if they are equal, a positive int if the first one is bigger than the second, else a negative int</returns>
        public int Compare(int n1, int n2)
        {
            return n1.CompareTo(n2);
        }

        /// <summary>
        /// Gives the exact difference between two integers
        /// </summary>
        /// <returns>The difference between the two items</returns>
        public int Difference(int n1, int n2)
        {
            return n1 - n2;
        }
    }
}
