using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odiou;
using WindowsInput;

namespace Controller
{
    /// <summary>
    /// Represents an association between one key and one note
    /// </summary>
    class NoteControl
    {
        /// <summary>
        /// Associated note
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Associated key
        /// </summary>
        public VirtualKeyCode Key { get; set; }

        /// <summary>
        /// If true the key will be kept pressed till the note changes
        /// </summary>
        public bool Keep { get; set; }

        /// <summary>
        /// Delay in milliseconds
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="note">The associated note</param>
        /// <param name="key">The associated key</param>
        /// <param name="keep">If true the key will be kept pressed till the note changesparam>
        /// <param name="delay">Delay in milliseconds</param>
        public NoteControl(RelativeNote note, VirtualKeyCode key, bool keep, int delay)
        {
            Note = note.ToString();
            Key = key;
            Keep = keep;
            Delay = delay;
        }
    }
}
