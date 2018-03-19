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
            List<string> logs = new List<string>();
            int num = 0;
            var path = getLogPath(ref num);

            if (num == 1)
            {

            }
            if (num > 1 && _file != path)
            {
                File.ReadAllLines(path);
            }

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] content = new byte[fs.Length];
            if (fs.CanRead)
            {
                StreamReader sr = new StreamReader(fs);
                long dataLengthToRead = fs.Length;//获取新的文件总大小

                if (dataLengthToRead > 0 && dataLengthToRead > _pos)
                {
                    fs.Seek(_pos, SeekOrigin.Begin);
                    int lengthRead = fs.Read(content, 0, Convert.ToInt32(dataLengthToRead - _pos));//读取的大小
                    var lines = Encoding.Default.GetString(content);//载入文本
                    _pos = dataLengthToRead;
                }

                sr.Close();
                fs.Close();
            }
            

            return logs.ToArray();
        }
    }
}
