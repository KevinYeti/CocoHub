using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;

namespace Logger.Cocohub.Core
{
    public static class LogReader
    {
        private static long _pos = 0;
        private static string _file = string.Empty;
        private static string _directory = "log";

        public static long LastPosition
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public static string LastFile
        {
            get { return _file; }
            set { _file = value; }
        }

        private static SortedSet<string> getLogs()
        {
            if (!Directory.Exists(_directory))
                return new SortedSet<string>();

            //
            var files = Directory.GetFiles(_directory, "cocohub.*.log", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                return new SortedSet<string>();
            else
                return new SortedSet<string>(files);
        }

        public static string[] Fetch()
        {
            var files = getLogs();
            int num = files.Count;

            if (num <= 0)
                return null;

            string path = files.Min;

            if (num == 1)
            {
                //try to remember the latest filename, and reset the pos when filename changed.
                if (_file != path)
                {
                    _pos = 0;
                    _file = path;
                }

                var logs = ReadLines(path, ref _pos);
                return logs;
            }
            else if (num > 11)
            {
                List<string> logs = new List<string>();
                string[] lines = null;
                //finish reading 1st file (from last read postion & read to EOF)
                if (File.Exists(path))
                {
                    var file = new FileInfo(path);
                    if (_file == path && file.Length <= _pos)
                    {
                        //skip this file if this file has read to EOF
                        Rename(path, path.Replace(".log", ".nut"));
                    }
                    else
                    {
                        //try to remember the latest filename, and reset the pos when filename changed.
                        if (_file != path)
                        {
                            _pos = 0;
                            _file = path;
                        }
                        lines = ReadLines(path, ref _pos);

                        if (lines != null && lines.Length > 0)
                            logs.AddRange(lines);
                        Rename(path, path.Replace(".log", ".nut"));
                        lines = null;
                    }
                }

                //finish reading 2nd~10th files
                for (int i = 1; i < 11; i++)
                {
                    path = files.ElementAt(i);
                    //try to remember the latest filename, and reset the pos when filename changed.
                    if (_file != path)
                    {
                        _file = path;
                    }
                    var contents = File.ReadLines(path);

                    logs.AddRange(contents);
                    Rename(path, path.Replace(".log", ".nut"));
                }

                if (lines != null && lines.Length > 0)
                    logs.AddRange(lines);

                return logs.ToArray();
            }
            else    //num >= 2 && num < 11
            {
                List<string> logs = new List<string>();
                string[] lines = null;

                //finish reading 1st file (from last read postion & read to EOF)
                if (File.Exists(path))
                {
                    var file = new FileInfo(path);
                    if (_file == path && file.Length <= _pos)
                    {
                        //skip this file if this file has read to EOF
                        Rename(path, path.Replace(".log", ".nut"));
                    }
                    else
                    {
                        //try to remember the latest filename, and reset the pos when filename changed.
                        if (_file != path)
                        {
                            _pos = 0;
                            _file = path;
                        }
                        lines = ReadLines(path, ref _pos);

                        if (lines != null && lines.Length > 0)
                            logs.AddRange(lines);
                        Rename(path, path.Replace(".log", ".nut"));
                        lines = null;
                    }
                }
                
                //finish reading 2nd~(num-1)th files
                for (int i = 1; i < (num - 1); i++)
                {
                    path = files.ElementAt(i);
                    //try to remember the latest filename, and reset the pos when filename changed.
                    if (_file != path)
                    {
                        _file = path;
                    }
                    var contents = File.ReadLines(path);

                    logs.AddRange(contents);
                    Rename(path, path.Replace(".log", ".nut"));
                }

                //keep reading (num)th file (from start)
                path = files.Max;
                //try to remember the latest filename, and reset the pos when filename changed.
                _file = path;
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

                //System.IO.File.AppendAllText("log/read.log", lines);

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
            var file = new FileInfo(srcFile);
            file.MoveTo(destFile);
        }
    }
}
