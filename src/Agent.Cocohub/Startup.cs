using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Agent.Cocohub
{
    public static class Startup
    {
        private static Thread _thread;

        public static void StartWatch()
        {
            if (_thread != null)
            {
                Console.WriteLine("Agent.Cocohub has already started.");
                return;
            }
            else
                internalStart();
        }

        public static void Stop()
        {
            if (_thread != null)
            {
                Console.WriteLine("Agent.Cocohub stopping.");
                AgentThread._running = false;
            }
            Console.WriteLine("Agent.Cocohub stopped.");
        }

        private static void internalStart()
        {
            _thread = new Thread(new ThreadStart(AgentThread.CoreFunc));
            _thread.IsBackground = true;
            AgentThread._running = true;
            _thread.Start();
            Console.WriteLine("Agent.Cocohub started.");
        }
    }
}
