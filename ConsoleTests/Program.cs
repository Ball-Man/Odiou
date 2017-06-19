using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;
using System.IO;
using Odiou;
using WindowsInput;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            while(Console.ReadLine() == "")
            {
                Console.WriteLine("Pressing ...");
                Thread.Sleep(1000);

                InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_A);
                InputSimulator.SimulateKeyDown(VirtualKeyCode.VK_B);
                Console.ReadLine();

                Console.WriteLine("Releasing ...");
                Thread.Sleep(1000);

                InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_A);
                InputSimulator.SimulateKeyUp(VirtualKeyCode.VK_B);
            }
        }
    }
}
