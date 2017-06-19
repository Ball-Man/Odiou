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
        public AudioRecorder Recorder { get; set; }

        /// <summary>
        /// Number of bits used as size of the FFT buffer(2^bits)
        /// </summary>
        public int PrecisionBits { get; set; }

        /// <summary>
        /// used to execute commands
        /// </summary>
        public NoteInterpreter Interpreter { get; set; } = new NoteInterpreter();

        /// <summary>
        /// Creates a controller
        /// </summary>
        /// <param name="recorder">The audio device used by the controller</param>
        public LiveAudioController(AudioRecorder recorder, TextBlock txtNote, Dispatcher dispatcher)
        {
            Recorder = recorder;

            Recorder.BufferGotData += (s, e) =>
            {
                Complex[] buffer;
                switch(Recorder.BitDepth)
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


                Array.Resize(ref buffer, 16384);
                Array.Sort(Enumerable.Range(0, buffer.Length).ToArray(), buffer, new ReverseBitSorting(14));
                
                //Gets data
                TransformedVector transformed = FFT.Transform(buffer, Recorder.SampleRate);
                Note note = FFT.GetNote((int)Math.Round(transformed.Carrier));

                //Updates UI
                dispatcher.Invoke(() => txtNote.Text = (note.ToString() == "C0"?" ":note.ToString()));

                //Execute the command
                Interpreter.Execute(FFT.GetNote((int)Math.Round(transformed.Carrier)));
            };
        }

        /// <summary>
        /// Stops listening
        /// </summary>
        public void Stop()
        {
            Recorder.Stop();
        }

        /// <summary>
        /// Starts listening
        /// </summary>
        public void Start()
        {
            Recorder.Start();
        }

        private Complex[] ToLong(byte[] bytes, int count, int channels)
        {
            Complex[] buffer = new Complex[count / (8 * channels)];
            for (int i = (8 * channels - 1); i < count; i += 8 * channels)
                buffer[(i + 1) / (8 * channels ) - 1] = (long)((bytes[i] << 56) | (bytes[i - 1] << 48) | (bytes[i - 2] << 40) | (bytes[i - 3] << 32) | (bytes[i - 4] << 24) | (bytes[i - 5] << 16) | (bytes[i - 6] << 8) | bytes[i - 7]);

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
