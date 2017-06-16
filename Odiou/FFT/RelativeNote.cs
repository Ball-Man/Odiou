using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odiou
{
    /// <summary>
    /// Represents one relative note(C, C#, D, etc.)
    /// </summary>
    public class RelativeNote : IEquatable<RelativeNote>
    {
        /// <summary>
        /// The char representing the wanted note(eg. 'C')
        /// </summary>
        public char Note { get; set; } 
        
        /// <summary>
        /// Tells if the note is sharp or not
        /// </summary>
        public bool IsSharp { get; set; }

        /// <summary>
        /// Creates a relative note
        /// </summary>
        /// <param name="note">Char representing the given note</param>
        /// <param name="sharp">Tells if the note is sharp or not</param>
        public RelativeNote(char note, bool sharp)
        {
            Note = note;
            IsSharp = sharp;
        }

        /// <summary>
        /// Parses a relative note form a string
        /// </summary>
        /// <param name="str">The note-format string</param>
        public static RelativeNote Parse(string str)
        {
            return new RelativeNote(str[0], str.Length >= 2 && str[1] == '#' ? true : false);
        }

        /// <summary>
        /// Makes possible equalizing relative notes
        /// </summary>
        public bool Equals(RelativeNote n)
        {
            return Note == n.Note && IsSharp == n.IsSharp;
        }

        /// <summary>
        /// String format for the note
        /// </summary>
        public override string ToString()
        {
            return Note + (IsSharp ? "#" : "");
        }
    }
}
