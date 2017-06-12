using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Odiou
{
    /// <summary>
    /// Class used to manage a frequency domain signal
    /// </summary>
    public class TransformedVector
    {
        /// <summary>
        /// Correspondence [frequency] = value
        /// </summary>
        public SortedBiDictionary<int, Complex> Values { get; }

        /// <summary>
        /// Gets the carrier frequency of the signal from the spectrum
        /// </summary>
        public double Carrier
        {
            get
            {
                var values = Values.Values.Select(x => x.Magnitude);
                var keys = Values.Keys;

                return keys[values.ToList().IndexOf(values.Max())];
            }
        }

        /// <summary>
        /// Creates an instance of the transformed vector
        /// </summary>
        /// <param name="values">Array containing the complex values of the transformed vector</param>
        /// <param name="freq">The sample frequency of the original signal</param>
        public TransformedVector(Complex[] values, int freq)
        {
            Values = new SortedBiDictionary<int, Complex>(Enumerable.Range(0, values.Length).Select(x => x * freq / values.Length), values);
        }
    }
}
