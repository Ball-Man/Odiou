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
        public int Compare(int a, int b)
        {
            int bits = (int)(Math.Log(Math.Max(a, b)) / Math.Log(2));

            int ra = 0, rb = 0;
            for (int i = 1; i <= bits; i++)
            {
                if ((a & 1 << (bits - i)) != 0)
                    ra |= 1 << (i - 1);

                if ((b & 1 << (bits - i)) != 0)
                    rb |= 1 << (i - 1);
            }

            if (ra > rb)
                return 1;
            else if (ra < rb)
                return -1;
            return 0;
        }
    }
}
