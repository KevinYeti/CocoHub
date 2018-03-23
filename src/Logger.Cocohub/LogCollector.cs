using Logger.Cocohub.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        //public static string[] Fetch()
        //{
        //    //
        //    int count = _collector.Count;
        //    if (count == 0)
        //        return null;

        //    string[] result = new String[count];
        //    string take;

        //    //DO NOT try to use parallel here 
        //    for (int i = 0; i < count; i++)
        //    {
        //        _collector.TryTake(out take);
        //        result[i] = take.Replace("\r", string.Empty).Replace("\n", string.Empty);
        //    }

        //    return result;
        //}

        public static string Fetch()
        {
            //
            int count = _collector.Count;
            if (count == 0)
                return null;
            else if (count > 1000)
                count = 1000;

            string result = string.Empty;

            Parallel.For(0, count, (i) => {
                if (_collector.TryTake(out string take) && !string.IsNullOrEmpty(take))
                {
                    result += take.Replace("\r", string.Empty).Replace("\n", string.Empty) + "\r\n";
                }
            });

            return result;
        }
    }
}
