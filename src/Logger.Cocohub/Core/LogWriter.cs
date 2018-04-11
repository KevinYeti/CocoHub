using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger.Cocohub.Core
{
    public static class LogWriter
    {
        private static string _directory = "log";
        private static string _path = _directory + "/cocohub.{0}.log";
        private static int _length = 1000 * 1000 * 10;      //10M

        private static string getLogPath() 
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            //
            var files = Directory.GetFiles(_directory, "cocohub.*.log", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                return string.Format(_path, DateTime.Now.ToString("yyyyMMdd.HHmmss.fff"));
            else
            {
                SortedSet<string> set = new SortedSet<string>(files);
                var latest = set.Max;
                if (new FileInfo(latest).Length > _length)
                    return string.Format(_path, DateTime.Now.ToString("yyyyMMdd.HHmmss.fff"));
                else
                    return latest;
            }
        }


        public static void Flush(string logs)
        {
            if (logs == null || logs.Length == 0)
                return;

            var path = getLogPath();
            File.AppendAllText(path, logs);

        }
    }
}
