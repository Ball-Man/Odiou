using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Odiou;
using System.Numerics;
using System.Windows.Threading;
using System.Windows;

namespace Controller
{
    /// <summary>
    /// Manages the live audio interpreter
    /// </summary>
    class LiveAudioController
    {
        /// <summary>
        /// The recorder used
        /// </summary>
        private AudioRecorder _recorder;

        /// <summary>
        /// Number of bits used as size of the FFT buffer(2^bits)
        /// </summary>
        public int PrecisionBits { get; set; }

        /// <summary>
        /// Creates a controller
        /// </summary>
        /// <param name="recorder">The audio device used by the controller</param>
        public LiveAudioController(AudioRecorder recorder, TextBlock txtNote, Dispatcher dispatcher)
        {
            _recorder = recorder;

            _recorder.BufferGotData += (s, e) =>
            {
                Complex[] buffer = new Complex[e.Bytes / 8];
                for (int i = 7; i < e.Bytes; i += 8)
                    buffer[(i + 1) / 8 - 1] = (int)((e.Buffer[i] << 24) | (e.Buffer[i - 1] << 16) | (e.Buffer[i - 2] << 8) | e.Buffer[i - 3]);

                Array.Resize(ref buffer, 16384);
                Array.Sort(Enumerable.Range(0, buffer.Length).ToArray(), buffer, new ReverseBitSorting(14));
                //Task.Factory.StartNew(() =>
                TransformedVector transformed = FFT.Transform(buffer, 48000);
                dispatcher.Invoke(() => txtNote.Text = (FFT.GetNote((int)Math.Round(transformed.Carrier))).ToString());
            };
        }

        /// <summary>
        /// Stops listening
        /// </summary>
        public void Stop()
        {
            _recorder.Stop();
        }

        /// <summary>
        /// Starts listening
        /// </summary>
        public void Start()
        {
            _recorder.Start();
        }
    }
}
