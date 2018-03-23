using Agent.Cocohub;
using System;

namespace Proxy.Cocohub
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
