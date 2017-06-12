using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace Odiou
{
    /// <summary>
    /// Class containing the method(s) useful for computing FFT
    /// </summary>
    public static class FFT
    {
        /// <summary>
        /// Field filled with [frequency] = note correspondence
        /// </summary>
        static private SortedBiDictionary<int, Note> _notes = new SortedBiDictionary<int, Note>();

        /// <summary>
        /// Transforms the vector using FFT algorithm
        /// </summary>
        /// <param name="values">The values of the time domain signal. Length must be a power of 2</param>
        /// <param name="freq">The sampling frequency</param>
        /// <returns>The tranformed vector(TransformedVector)</returns>
        public static TransformedVector Transform(Complex[] values, int freq)
        {
            Complex[] buffer = (Complex[])values.Clone();

            int cycles = (int)(Math.Log(buffer.Length) / Math.Log(2));
            for (int c = 1; c <= cycles; c++)
            {
                int step = (int)Math.Pow(2, c);
                for (int i = 0; i < buffer.Length; i += step)
                {
                    for (int s = 0; s < step / 2; s++)
                    {
                        Complex even = buffer[i + s];
                        Complex odd = buffer[i + s + (step / 2)];

                        double w = -2 * Math.PI * s / step;
                        odd *= new Complex(Math.Cos(w), Math.Sin(w));

                        buffer[i + s] = even + odd;
                        buffer[i + s + (step / 2)] = even - odd;

                    }
                }
            }

            return new TransformedVector(buffer, freq);
        }

        /// <summary>
        /// Charges notes from given file
        /// </summary>
        /// <param name="fileName">File path</param>
        public static void ChargeNotes(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (reader.Peek() > 0)
                {
                    string[] tmp = reader.ReadLine().Split(':');
                    _notes[int.Parse(tmp[0])] = Note.Parse(tmp[1]);  
                }
            }
        }

        /// <summary>
        /// Gets the nearest note to the given frequency
        /// </summary>
        /// <param name="freq">The given frequency(Hz)</param>
        /// <returns>A Note instance representing the given frequency</returns>
        public static Note GetNote(int freq)
        {
            return _notes.GetNearestValue(freq);
        }
    }
}
