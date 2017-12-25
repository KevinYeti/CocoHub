using Logger.Cocohub.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logger.Cocohub
{
    public static class LogCollector
    {
        public static CoreCollector _collector = new CoreCollector();

        public static void Add(string log)
        {
            _collector.Add(log);
        }

        public static int Count()
        {
            return _collector.Count;
        }

        public static string[] Fetch()
        {
            //
            int count = _collector.Count;
            if (count == 0)
                return null;

            string[] result = new String[count];
            string take;
            for (int i = 0; i < count; i++)
            {
                _collector.TryTake(out take);
                result[i] = take;
            }

            return result;
        }
    }
}
