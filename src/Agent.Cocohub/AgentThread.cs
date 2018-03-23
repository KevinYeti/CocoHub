using Agent.Cocohub.Entity;
using Agent.Cocohub.Log;
using Agent.Cocohub.PgProvider;
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
                try
                {
                    var logs = LogReader.Fetch();
                    if (logs == null || logs.Length == 0)
                    {
                        Thread.Sleep(3000);
                        continue;
                    }

                    Parallel.ForEach(logs, (log) => { _logs.Add(log); });

                    if (logs.Length >= 1000)
                        Thread.Sleep(1000);
                    else
                        Thread.Sleep(3000);
                }
                catch(Exception ex) 
                {
                    Console.WriteLine("Log agent error:CoreFetch. " + ex.Message);
                    Thread.Sleep(10000);
                }
            }
        }

        internal static void CoreWrite()
        {
            while (_running || _logs.Count > 0)
            {
                try
                {
                    int loop = _logs.Count > 1000 ? 1000 : _logs.Count;
                    if (loop == 0)
                    {
                        Thread.Sleep(3000);
                        continue;
                    }
                    Console.WriteLine(loop.ToString() + " lines fetched.");
                    List<LogEntity> entities = new List<LogEntity>();
                    Parallel.For(0, loop, (i) => {
                        if (_logs.TryTake(out var log) && !string.IsNullOrEmpty(log))
                        {
                            if (LogResolver.TryResolve(log, out var entity) && entity != null)
                                entities.Add(entity);
                        }
                    });

                    PgWriter.Write2Db(entities);

                    if (loop > 1000)
                        Thread.Sleep(100);
                    else
                        Thread.Sleep(3000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Log agent error:CoreWrite. " + ex.Message);
                    Thread.Sleep(10000);
                }
            }

            Console.WriteLine("Agent.Cocohub stopped.");
        }
    }
}
