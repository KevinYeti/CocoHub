using Agent.Cocohub.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Agent.Cocohub
{
    public static class Startup
    {
        private static Thread _threadFetch;
        private static Thread _threadWriteDB;

        public static void StartWatch(Action<IEnumerable<LogEntity>> write)
        {
            if (write == null)
            {
                Console.WriteLine("Agent.Cocohub has no writer.");
                return;
            }
            else
                AgentThread._write = write;

            if (_threadFetch != null || _threadWriteDB != null)
            {
                Console.WriteLine("Agent.Cocohub has already started.");
                return;
            }
            else
                internalStart();
        }

        public static void Stop()
        {
            Console.WriteLine("Agent.Cocohub stopping.");
            AgentThread._running = false;
        }

        private static void internalStart()
        {
            AgentThread._running = true;

            _threadFetch = new Thread(new ThreadStart(AgentThread.CoreFetch));
            _threadFetch.IsBackground = true;
            _threadFetch.Start();

            _threadWriteDB = new Thread(new ThreadStart(AgentThread.CoreWrite));
            _threadWriteDB.IsBackground = true;
            _threadWriteDB.Start();

            Console.WriteLine("Agent.Cocohub started.");
        }
    }
}
