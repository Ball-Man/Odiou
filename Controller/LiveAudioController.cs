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
                Complex[] buffer = new Complex[e.Bytes / 8];
                for (int i = 7; i < e.Bytes; i += 8)
                    buffer[(i + 1) / 8 - 1] = (int)((e.Buffer[i] << 24) | (e.Buffer[i - 1] << 16) | (e.Buffer[i - 2] << 8) | e.Buffer[i - 3]);

                Array.Resize(ref buffer, 16384);
                Array.Sort(Enumerable.Range(0, buffer.Length).ToArray(), buffer, new ReverseBitSorting(14));
                
                //Gets data
                TransformedVector transformed = FFT.Transform(buffer, 48000);
                Note note = FFT.GetNote((int)Math.Round(transformed.Carrier));

                //Updates UI
                dispatcher.Invoke(() => txtNote.Text = (note.ToString() == "C0"?"-":note.ToString()));

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
    }
}
