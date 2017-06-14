using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odiou;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string x in AudioRecorder.Devices)
                Console.WriteLine(x);

            AudioRecorder recorder = new AudioRecorder(int.Parse(Console.ReadLine()));
            recorder.BufferGotData += (s, e) => Console.WriteLine(e.Bytes);

            recorder.Start();
            Console.ReadKey();
            recorder.Stop();
        }
    }
}
