using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Odiou
{
    /// <summary>
    /// High level class for NAudio.WasapiCapture
    /// </summary>
    public class AudioRecorder
    {
        /// <summary>
        /// String array containing the available audio recording devices' names
        /// </summary>
        public static string[] Devices
        {
            get
            {
                return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).Select(x => x.FriendlyName).ToArray();
            }
        }

        /// <summary>
        /// The actual audio recording class from NAudio
        /// </summary>
        private WasapiCapture _recorder;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="id">The selected device's index(from the Devices array)</param>
        /// <param name="bufferSize">Audio buffer size in milliseconds</param>
        public AudioRecorder(int id, int bufferSize)
        {
            _recorder = new WasapiCapture(new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[id], false, bufferSize);
            _recorder.DataAvailable += (s, e) => BufferFull.Invoke(s, new AudioEventArgs(e));
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="id">The selected device's index(from the Devices array)</param>
        public AudioRecorder(int id) : this(id, 100) { }

        /// <summary>
        /// Improved constructor, specifies the wanted wave format
        /// </summary>
        /// <param name="id">The selected device's index(from the Devices array)</param>
        /// <param name="sampleFreq">Sampling frequency(Hz)</param>
        /// <param name="bits">Bit depth</param>
        /// <param name="channels">Number of channels</param>
        public AudioRecorder(int id, int sampleFreq, int bits, int channels) : this(id)
        {
            _recorder.WaveFormat = new WaveFormat(sampleFreq, bits, channels);
        }

        /// <summary>
        /// Starts recording
        /// </summary>
        public void Start()
        {
            _recorder.StartRecording();
        }

        /// <summary>
        /// Stops recording
        /// </summary>
        public void Stop()
        {
            _recorder.StopRecording();
        }

        /// <summary>
        /// Occurs when the audio buffer is full and it needs to be processed
        /// </summary>
        public event EventHandler<AudioEventArgs> BufferFull;
    }
}
