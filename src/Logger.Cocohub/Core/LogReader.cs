using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Logger.Cocohub.Core
{
    public static class LogReader
    {
        private static long _pos = 0;
        private static string _file = string.Empty;
        private static string _directory = "log";

        private static string getLogPath(ref int num)
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
                num = set.Count;
                return set.Min;
            }
        }

        public static string[] Fetch()
        {
            int num = 0;
            var path = getLogPath(ref num);

            //try to remember the latest filename, and reset the pos when filename changed.
            if (_file != path)
            {
                _pos = 0;
                _file = path;
            }

            if (num <= 0)
                return null;
            else if (num == 1)
            {
                var logs = ReadLines(path, ref _pos);
                System.IO.File.AppendAllText("log/fetch.log", "fetch1:" + path + " pos:" + _pos + Environment.NewLine);
                System.IO.File.AppendAllText("log/fetch.log", "logs output" + Environment.NewLine);
                return logs;
            }
            else    //num >= 2
            {
                List<string> logs = new List<string>();

                //finish reading 1st file (from last read)
                var lines = ReadLines(path, ref _pos);

                System.IO.File.AppendAllText("log/fetch.log", "fetch2.1:" + path + " pos:" + _pos + Environment.NewLine);
                System.IO.File.AppendAllLines("log/fetch.log", lines);

                if (lines != null && lines.Length > 0)
                    logs.AddRange(lines);
                Rename(path, path.Replace(".log", ".nut"));
                lines = null;

                //finish reading 2nd~(num-1)th file
                while (num > 2)
                {
                    path = getLogPath(ref num);
                    var contents = File.ReadLines(path);

                    System.IO.File.AppendAllText("log/fetch.log", "fetch2.2:" + path + " pos:" + _pos + Environment.NewLine);
                    System.IO.File.AppendAllText("log/fetch.log", "contents output" + Environment.NewLine);

                    logs.AddRange(lines);
                    Rename(path, path.Replace(".log", ".nut"));
                }

                //keep reading (num)th file (from start)
                path = getLogPath(ref num);
                _pos = 0;
                lines = ReadLines(path, ref _pos);

                System.IO.File.AppendAllText("log/fetch.log", "fetch2.3:" + path + " pos:" + _pos + Environment.NewLine);
                System.IO.File.AppendAllText("log/fetch.log", "lines output" + Environment.NewLine);

                if (lines != null && lines.Length > 0)
                    logs.AddRange(lines);

                return logs.ToArray();
            }
        }

        private static string[] ReadLines(string path, ref long pos)
        {
            string lines = string.Empty;
            if (!File.Exists(path))
                return null;

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                if (fs.CanRead)
                {
                    StreamReader sr = new StreamReader(fs);
                    long dataLengthToRead = fs.Length;//获取新的文件总大小
                    byte[] content = new byte[dataLengthToRead - pos];

                    if (dataLengthToRead > 0 && dataLengthToRead > pos)
                    {
                        fs.Seek(pos, SeekOrigin.Begin);
                        int lengthRead = fs.Read(content, 0, Convert.ToInt32(dataLengthToRead - pos));//读取的大小
                        lines = Encoding.Default.GetString(content);//载入文本
                        pos = dataLengthToRead;
                    }

                    sr.Close();
                    fs.Close();
                }

                System.IO.File.AppendAllText("log/read.log", lines);

                if (string.IsNullOrEmpty(lines))
                    return null;
                else
                    return lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadLines error: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        private static void Rename(string srcFile, string destFile)
        {
            int count = 0;
            while (!File.Exists(destFile))
            {
                if (File.Exists(srcFile) && count < 10 )
                {
                    count++;
                    File.Move(srcFile, destFile);
                    Thread.Sleep(3000);         //Sleep 3 seconds to ensure success
                }
                else
                    return;
            }
        }
    }
}
