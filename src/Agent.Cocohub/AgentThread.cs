using Logger.Cocohub.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.Cocohub
{
    public static class AgentThread
    {
        internal static bool _running;
        public static void CoreFunc()
        {
            while (_running)
            {
                var logs = LogReader.Fetch();
                LogWriter.Flush(logs);

                if (LogCollector.Count() > 300)
                    Thread.Sleep(1000);
                else
                    Thread.Sleep(3000);     //DEBUG:set to 1000 when debuging so that you can get the result immediately.
            }
        }
    }
}
