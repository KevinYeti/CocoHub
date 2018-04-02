using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Logger.Cocohub.Core
{
    public static class LogReader
    {
        private static long _pos = 0;
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

            if (num <= 0)
                return null;
            else if (num == 1)
                return ReadLines(path, ref _pos);
            else    //num >= 2
            {
                List<string> logs = new List<string>();

                //finish reading 1st file (from last read)
                var lines = ReadLines(path, ref _pos);
                if (lines != null && lines.Length > 0)
                    logs.AddRange(lines);
                File.Move(path, path.Replace(".log", ".nut"));
                lines = null;

                //finish reading 2nd~(num-1)th file
                while (num > 2)
                {
                    path = getLogPath(ref num);
                    logs.AddRange(File.ReadLines(path));
                    File.Move(path, path.Replace(".log", ".nut"));
                }

                //keep reading (num)th file (from start)
                path = getLogPath(ref num);
                _pos = 0;
                lines = ReadLines(path, ref _pos);
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

                if (string.IsNullOrEmpty(lines))
                    return null;
                else
                    return lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ReadLines error: " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
    }
}
