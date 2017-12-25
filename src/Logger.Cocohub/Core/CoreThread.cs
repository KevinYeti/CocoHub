using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Logger.Cocohub.Core
{
    public static class CoreThread
    {
        public static void CoreFunc()
        {
            while (true)
            {
                var logs= LogCollector.Fetch();
                LogWriter.Flush(logs);

                if (LogCollector.Count() > 3000)
                    Thread.Sleep(1000);
                else
                    Thread.Sleep(1000);
            }
        }
    }
}
