﻿using System;
using System.Collections.Generic;
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
                if(Associations[i].Note == note.ToString())
                {
                    if (Associations[i].Note != _last.ToString())
                    {
                        NoteControl association = Associations[i];

                        //Key up for all the keys which where pressed
                        while (_pushed.Count > 0)
                            InputSimulator.SimulateKeyUp(_pushed.Dequeue());

                        //ThreadPool.QueueUserWorkItem((state) =>
                        {
                            Thread.Sleep(association.Delay);
                            if (association.Keep)
                            {
                                InputSimulator.SimulateKeyDown(association.Key);
                                _pushed.Enqueue(association.Key);
                            }
                            else
                                InputSimulator.SimulateKeyPress(association.Key);
                        };
                    }
                }
                _last = note;
            }
        }
    }
}
