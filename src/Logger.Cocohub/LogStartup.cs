using Logger.Cocohub.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Logger.Cocohub
{
    public static class LogStartup
    {
        private static Thread _thread;

        public static void Start()
        {
            if (_thread == null)
            {
                _thread = new Thread( new ThreadStart(CoreThread.CoreFunc));
                _thread.IsBackground = true;
                _thread.Start();
            }
        }
    }
}
