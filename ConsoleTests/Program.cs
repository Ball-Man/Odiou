using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using Odiou;

namespace ConsoleTests
{
    class Program
    {
        //Good for my tests
        const int FREQ = 48000;
        const int LEN = 65536;

        static void Main(string[] args)
        {
            Console.Write("File: ");
            StreamReader reader = new StreamReader(Console.ReadLine());

            List<double> list = new List<double>();

            while (reader.Peek() > 0)
            {
                string tmp = reader.ReadLine();
                list.AddRange(tmp.TrimEnd().Split(' ').Select(x => double.Parse(x)));
            }
            reader.Close();

            Complex[] buffer = new Complex[LEN];
            Array.Copy(list.Select(x => new Complex(x, 0)).ToArray(), buffer, list.Count);

            int[] keys = Enumerable.Range(0, buffer.Length).ToArray();
            Array.Sort(keys, buffer, new ReverseBitSorting());

            TransformedVector vv = FFT.Transform(buffer, FREQ);
            Console.WriteLine(vv.Carrier);
            Console.ReadKey();
        }
    }
}
