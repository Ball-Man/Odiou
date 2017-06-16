using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Odiou;
using System.Numerics;
using System.Windows.Threading;

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
        /// Button used to start listening
        /// </summary>
        private Button _btnStart;

        /// <summary>
        /// Button used to stop listening
        /// </summary>
        private Button _btnStop;

        /// <summary>
        /// Creates a controller
        /// </summary>
        /// <param name="recorder">The audio device used by the controller</param>
        /// <param name="btnStart">The button used to start the live execution</param>
        /// <param name="btnStop">The button used to stop the live execution</param>
        public LiveAudioController(AudioRecorder recorder, Button btnStart, Button btnStop, TextBlock txtNote, Dispatcher dispatcher)
        {
            _recorder = recorder;
            
            _btnStart = btnStart;
            _btnStop = btnStop;

            btnStart.Click += (s,e) => Start();
            btnStop.Click += (s, e) => Stop();

            _recorder.BufferGotData += (s, e) =>
            {
                int[] wave = new int[e.Bytes / 8];
                for (int i = 7; i < e.Bytes; i += 8)
                    wave[(i + 1) / 8 - 1] = (e.Buffer[i] << 24) + (e.Buffer[i - 1] << 16) + (e.Buffer[i - 2] << 8) + e.Buffer[i - 3];

                Complex[] buffer = wave.Select(x => new Complex(x, 0)).ToArray();
                Array.Resize(ref buffer, 16384);
                Array.Sort(Enumerable.Range(0, buffer.Length).ToArray(), buffer, new ReverseBitSorting(14));
                //Task.Factory.StartNew(() =>
                TransformedVector transformed = FFT.Transform(buffer, 48000);
                dispatcher.Invoke(() => txtNote.Text = (FFT.GetNote((int)transformed.Carrier)).ToString());
            };
        }

        /// <summary>
        /// Stops listening
        /// </summary>
        public void Stop()
        {
            _btnStop.IsEnabled = false;
            _btnStart.IsEnabled = true;
            _recorder.Stop();
        }

        /// <summary>
        /// Starts listening
        /// </summary>
        public void Start()
        {
            _btnStart.IsEnabled = false;
            _btnStop.IsEnabled = true;
            _recorder.Start();
        }
    }
}
