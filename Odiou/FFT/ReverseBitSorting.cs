using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odiou
{
    /// <summary>
    /// Helper class for the reverse bit sorting
    /// </summary>
    public class ReverseBitSorting : IComparer<int>
    {

        /// <summary>
        /// Bits of the max reachable value for the session
        /// </summary>
        private int _bits;

        /// <summary>
        /// Implementation of IComparer, compares two integers based on their reverse form
        /// </summary>
        public int Compare(int a, int b)
        {
            int ra = 0, rb = 0;
            for (int i = 1; i <= _bits; i++)
            {
                if ((a & 1 << (_bits - i)) != 0)
                    ra |= 1 << (i - 1);

                if ((b & 1 << (_bits - i)) != 0)
                    rb |= 1 << (i - 1);
            }

            if (ra > rb)
                return 1;
            else if (ra < rb)
                return -1;
            return 0;
        }

        /// <summary>
        /// Constructor, requires the max number of bits for this session
        /// </summary>
        /// <param name="bits">The number of bits of the max value from the vector</param>
        public ReverseBitSorting(int bits)
        {
            _bits = bits;
        }
    }
}
