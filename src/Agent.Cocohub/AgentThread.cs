using Logger.Cocohub.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agent.Cocohub
{
    public static class AgentThread
    {
        internal static bool _running;

        internal static ConcurrentBag<string> _logs = new ConcurrentBag<string>();

        internal static void CoreFetch()
        {
            while (_running)
            {
                var logs = LogReader.Fetch();
                Parallel.ForEach(logs, (log) => { _logs.Add(log); });

                if (logs.Length >= 1000)
                    Thread.Sleep(1000);
                else
                    Thread.Sleep(3000);
            }
        }

        internal static void CoreWrite()
        {
            while (_running || _logs.Count > 0)
            {
                
                Thread.Sleep(1000);
            }
            Console.WriteLine("Agent.Cocohub stopped.");
        }
    }
}
