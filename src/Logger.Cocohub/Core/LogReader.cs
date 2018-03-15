using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger.Cocohub.Core
{
    public static class LogReader
    {
        private static string _file;
        private static int row;
        private static string _directory = "log";

        private static string getLogPath()
        {
            if (!Directory.Exists(_directory))
                return null;

            //
            var files = Directory.GetFiles(_directory, "cocohub.*.log", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                return null;
            else
            {
                SortedSet<string> set = new SortedSet<string>(files);
                return set.Min;
            }
        }

        public static string[] Fetch()
        {
            List<string> logs = new List<string>();


            return logs.ToArray();
        }
    }
}
