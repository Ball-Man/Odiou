using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows.Controls;
using Odiou;

namespace Controller
{
    /// <summary>
    /// Manages the analysis part of the app
    /// </summary>
    class AnalysisAudioController
    {
        /// <summary>
        /// Device used to record
        /// </summary>
        public AudioRecorder Recorder { get; set; }

        /// <summary>
        /// Canvas used by the canvas manager of the time domain
        /// </summary>
        private Canvas _timeCanvas;

        /// <summary>
        /// Canvas used bu the canvas manager of the frequency domain
        /// </summary>
        private Canvas _freqCanvas;

        /// <summary>
        /// Canvas manager for time domain
        /// </summary>
        private CanvasManager _timeManager;

        /// <summary>
        /// Canvas manager for frequency domain
        /// </summary>
        private CanvasManager _freqManager;

        /// <summary>
        /// Buffer where samples are stored
        /// </summary>
        private List<Complex> _buffer;

        /// <summary>
        /// Creates an analysis controller
        /// </summary>
        /// <param name="recorder"></param>
        /// <param name="canvas"></param>
        public AnalysisAudioController(AudioRecorder recorder, Canvas time, Canvas freq)
        {
            Recorder = recorder;
            _timeCanvas = time;
            _freqCanvas = freq;
            _buffer = new List<Complex>();

            Recorder.BufferGotData += (s, e) =>
            {
                Complex[] buffer;
                switch (Recorder.BitDepth)
                {
                    case 64:
                        {
                            buffer = ToLong(e.Buffer, e.Bytes, Recorder.Channels);
                            break;
                        }
                    case 32:
                        {
                            buffer = ToInt(e.Buffer, e.Bytes, Recorder.Channels);
                            break;
                        }
                    case 16:
                        {
                            buffer = ToShort(e.Buffer, e.Bytes, Recorder.Channels);
                            break;
                        }
                    default:
                        {
                            buffer = e.Buffer.Select(x => new Complex(x, 0)).ToArray();
                            break;
                        }
                }
                _buffer.AddRange(buffer);
            };
        }
        
        /// <summary>
        /// Starts recording
        /// </summary>
        public void Start()
        {
            _buffer = new List<Complex>();
            GC.Collect();

            Recorder.Start();
        }

        /// <summary>
        /// Stops recording and starts drawing
        /// </summary>
        public void Stop()
        {
            Recorder.Stop();
            
            double[] tmp = Compute();
            for (int i = 0; i < _buffer.Count; i++)
                if (i % 80 != 0)
                    _buffer.RemoveAt(i);
            double[] buffer = _buffer.Select(x => x.Real).ToArray();

            _timeManager = new CanvasManager(_timeCanvas, buffer, buffer.Max(), buffer.Min());
            _freqManager = new CanvasManager(_freqCanvas, tmp, tmp.Max(), 0);

            _timeManager.Clear();
            _freqManager.Clear();

            Draw();
        }

        /// <summary>
        /// Computes FFT with the internal buffer
        /// </summary>
        /// <returns>Array containing the magnitudes</returns>
        private double[] Compute()
        {
            var buffer = _buffer.ToArray();
            Array.Resize(ref buffer, 4096);
            Array.Sort(Enumerable.Range(0, buffer.Length).ToArray(), buffer, new ReverseBitSorting(12));
            return FFT.Transform(buffer, Recorder.SampleRate, false).Values.Values.Select(x => x.Magnitude).ToArray();
        }

        /// <summary>
        /// Triggers the two internal canvas managers
        /// </summary>
        private void Draw()
        {
            _timeManager.DrawPoints();
            _freqManager.DrawPoints();
        }

        private Complex[] ToLong(byte[] bytes, int count, int channels)
        {
            Complex[] buffer = new Complex[count / (8 * channels)];
            for (int i = (8 * channels - 1); i < count; i += 8 * channels)
                buffer[(i + 1) / (8 * channels) - 1] = (long)((bytes[i] << 56) | (bytes[i - 1] << 48) | (bytes[i - 2] << 40) | (bytes[i - 3] << 32) | (bytes[i - 4] << 24) | (bytes[i - 5] << 16) | (bytes[i - 6] << 8) | bytes[i - 7]);

            return buffer;
        }

        private Complex[] ToInt(byte[] bytes, int count, int channels)
        {
            Complex[] buffer = new Complex[count / (4 * channels)];
            for (int i = (4 * channels - 1); i < count; i += 4 * channels)
                buffer[(i + 1) / (4 * channels) - 1] = (int)((bytes[i] << 24) | (bytes[i - 1] << 16) | (bytes[i - 2] << 8) | bytes[i - 3]);

            return buffer;
        }

        private Complex[] ToShort(byte[] bytes, int count, int channels)
        {
            Complex[] buffer = new Complex[count / (2 * channels)];
            for (int i = (2 * channels - 1); i < count; i += 2 * channels)
                buffer[(i + 1) / (2 * channels) - 1] = (short)((bytes[i] << 8) | bytes[i - 1]);

            return buffer;
        }
    }
}
