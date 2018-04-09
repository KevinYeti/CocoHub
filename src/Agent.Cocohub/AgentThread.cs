using Agent.Cocohub.Entity;
using Agent.Cocohub.Log;
using Logger.Cocohub.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Agent.Cocohub
{
    public static class AgentThread
    {
        internal static bool _running;

        internal static ConcurrentQueue<string> _logs = new ConcurrentQueue<string>();

        internal static Action<IEnumerable<LogEntity>> _write;

        internal static void CoreFetch()
        {
            string nutFile = "_cocohub.log.nut";
            if (File.Exists(nutFile))
            {
                //try to restore the last read postion
                string[] lines = File.ReadAllLines(nutFile);
                if (lines.Length == 2)
                {
                    var lastFile = lines[0].Trim();
                    var lastPos = lines[1].Trim();
                    if (File.Exists(lastFile) && long.TryParse(lastPos, out long result))
                    {
                        LogReader.LastFile = lastFile;
                        LogReader.LastPosition = result;
                    }
                }
            }

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

                    Parallel.ForEach(logs, (log) => { _logs.Enqueue(log); });

                    if (logs.Length >= 1000)
                        Thread.Sleep(1000);
                    else
                        Thread.Sleep(3000);
                }
                catch(Exception ex) 
                {
                    Console.WriteLine("Log agent error:CoreFetch. " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Thread.Sleep(10000);
                }
            }

            File.WriteAllLines(nutFile, new string[] { LogReader.LastFile, LogReader.LastPosition.ToString() });
        }

        internal static void CoreWrite()
        {
            while (_running || _logs.Count > 0)
            {
                try
                {
                    int loop = _logs.Count > 10000 ? 10000 : _logs.Count;
                    if (loop == 0)
                    {
                        Thread.Sleep(3000);
                        continue;
                    }
                    List<LogEntity> entities = new List<LogEntity>();
                    for (int i = 0; i < loop; i++)
                    {
                        if (_logs.TryDequeue(out var log) && !string.IsNullOrEmpty(log))
                        {
                            if (LogResolver.TryResolve(log, out var entity) && entity != null)
                                entities.Add(entity);
                            else
                                Console.WriteLine("TryResolve error:" + log);
                        }
                        else
                            Console.WriteLine("Dequeue error.");
                    }

                    if (entities.Count > 0)
                        _write(entities);

                    if (loop >= 10000)
                        Thread.Sleep(100);
                    else
                        Thread.Sleep(3000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Log agent error:CoreWrite. " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Thread.Sleep(10000);
                }
            }

            Console.WriteLine("Agent.Cocohub stopped.");
        }
    }
}
