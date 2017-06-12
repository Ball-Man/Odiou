using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odiou
{
    /// <summary>
    /// Represents a note from a specific octave
    /// </summary>
    public class Note : RelativeNote, IEquatable<Note>
    {
        /// <summary>
        /// The note's octave
        /// </summary>
        public int Octave { get; set; }

        /// <summary>
        /// Creates a note from the information given
        /// </summary>
        /// <param name="note">The char representing the note</param>
        /// <param name="sharp">Tells if the note is sharp</param>
        /// <param name="octave">The belonging octave</param>
        public Note(char note, bool sharp, int octave) : base(note, sharp)
        {
            Octave = octave;
        }

        /// <summary>
        /// Creates a note from the information given
        /// </summary>
        /// <param name="note">The corresponding relative note</param>
        /// <param name="octave">The belonging octave</param>
        public Note(RelativeNote note, int octave) : this(note.Note, note.IsSharp, octave) { }

        /// <summary>
        /// Makes possible equalizing two notes
        /// </summary>
        public bool Equals(Note n)
        {
            return Note == n.Note && IsSharp == n.IsSharp && Octave == n.Octave;
        }

        /// <summary>
        /// Parses a note form a string
        /// </summary>
        /// <param name="str">The note-format string</param>
        public new static Note Parse(string str)
        {
            bool sharp = str.Length >= 2 && str[1] == '#' ? true : false;
            return new Note(str[0], sharp, int.Parse(str[sharp?2:1].ToString()));
        }

        /// <summary>
        /// String format for the note
        /// </summary>
        public override string ToString()
        {
            return base.ToString() + Octave;
        }
    }
}
