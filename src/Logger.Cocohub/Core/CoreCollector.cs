using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Logger.Cocohub.Core
{
    public class CoreCollector : ConcurrentQueue<string>
    {
        public void Add(string item)
        {
            base.Enqueue(item);
        }

        public bool TryTake(out string result)
        {
            return base.TryDequeue(out result);
        }
    }
}
