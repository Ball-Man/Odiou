using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Odiou
{
    /// <summary>
    /// Incapsulation event arguments for NAudio.WaveInEventArgs
    /// </summary>
    public class AudioEventArgs
    {
        /// <summary>
        /// Contains the recorded bytes
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// The number of recorded bytes
        /// </summary>
        public int Bytes { get; set; }

        /// <summary>
        /// Creates an argument from the given information
        /// </summary>
        /// <param name="buffer">The buffer of recorded bytes</param>
        /// <param name="bytes">The number of bytes recorded</param>
        public AudioEventArgs(byte[] buffer, int bytes)
        {
            Buffer = buffer;
            Bytes = bytes;
        }

        /// <summary>
        /// Creates an argument from the given information
        /// </summary>
        /// <param name="args">Object from which data is parsed(NAudio.WaveInEventArgs)</param>
        public AudioEventArgs(WaveInEventArgs args) : this(args.Buffer, args.BytesRecorded) { }
    }
}
