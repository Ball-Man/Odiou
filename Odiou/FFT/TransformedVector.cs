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
        public SortedBiDictionary<double, Complex> Values { get; }

        /// <summary>
        /// Gets the carrier frequency of the signal from the spectrum
        /// </summary>
        public double Carrier
        {
            get
            {
                var values = Values.Values.Select(x => x.Magnitude).ToList();
                var keys = Values.Keys;
                var max = values.Max();

                if (max < FilterThreshold)
                    return 0;
                return keys[values.IndexOf(max)];
            }
        }

        /// <summary>
        /// Represents the threshold under which frequencies values(magnitudes) are considered 0
        /// </summary>
        public long FilterThreshold { get; set; } = (long)15e10;

        /// <summary>
        /// Creates an instance of the transformed vector
        /// </summary>
        /// <param name="values">Array containing the complex values of the transformed vector</param>
        /// <param name="samples">Number of samples</param>
        /// <param name="freq">The sample frequency of the original signal</param>
        public TransformedVector(Complex[] values, int samples, int freq)
        {
            Values = new SortedBiDictionary<double, Complex>(Enumerable.Range(0, values.Length).Select(x => Math.Round(((double)x) * freq / samples, 2)), values);
        }
    }
}
