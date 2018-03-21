using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Cocohub
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup.StartWatch();
            Console.ReadKey();
        }
    }
}
