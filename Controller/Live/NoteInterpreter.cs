using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Odiou;
using WindowsInput;

namespace Controller
{
    /// <summary>
    /// Manages the note-key association
    /// </summary>
    class NoteInterpreter
    {
        /// <summary>
        /// List of commands
        /// </summary>
        public List<NoteControl> Associations { get; set; }

        /// <summary>
        /// Queue of the keys currently down
        /// </summary>
        private Queue<VirtualKeyCode> _pushed;

        /// <summary>
        /// Last note played
        /// </summary>
        private Note _last;

        /// <summary>
        /// Basic constructor
        /// </summary>
        public NoteInterpreter()
        {
            Associations = new List<NoteControl>();
            _last = new Note(new RelativeNote('c', false), 0);
            _pushed = new Queue<VirtualKeyCode>();
        }

        /// <summary>
        /// Executes commands relative to the given note
        /// </summary>
        /// <param name="note">The given note</param>
        public void Execute(Note note)
        {
            for(int i = 0; i < Associations.Count; i++)
            {
                NoteControl association = Associations[i];

                //Releases the kept keys
                if (!note.Equals(_last))
                    while (_pushed.Count > 0)
                        InputSimulator.SimulateKeyUp(_pushed.Dequeue());


                if (association.Note == note.ToString())
                {
                    if (!note.Equals(_last))
                    {
                        ThreadPool.QueueUserWorkItem((state) =>
                        {
                            Thread.Sleep(association.Delay);
                            if (association.Keep)
                            {
                                InputSimulator.SimulateKeyDown(association.Key);
                                _pushed.Enqueue(association.Key);
                            }
                            else
                                InputSimulator.SimulateKeyPress(association.Key);
                        });
                    }
                }
            }
            _last = note;
        }

        /// <summary>
        /// Serializes associations on an xml file
        /// </summary>
        /// <param name="filename">File path</param>
        public void Save(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true }))
            { 
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<NoteControl>));
                serializer.WriteObject(writer, Associations);
            }
        }

        /// <summary>
        /// Reads associations from an xml file
        /// </summary>
        /// <param name="filename">File path</param>
        public void Load(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
            using (XmlReader reader = XmlReader.Create(stream))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<NoteControl>));
                Associations = (List<NoteControl>)serializer.ReadObject(reader);
            }
        }
    }
}
