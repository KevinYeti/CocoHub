using Logger.Cocohub;
using System;
using System.Threading;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            LogStartup.Start();

            Foo(0, null);
            Foo(100, new Uri("http://www.baidu.com"));
            Console.WriteLine("Hello World!");

            Thread thd = new Thread(new ParameterizedThreadStart((obj) => {
                while (true)
                {
                    Foo(0, null);
                    Thread.Sleep(1000);
                }
            }));
            //thd.Start(Foo(100, null));

            ExpTest1();

            Console.ReadLine();
        }

        public static bool Foo(int bar, Uri uri)
        {
            if (bar >= 100)
                return true;
            else
                return false;
        }

        private static bool ExpTest1()
        {
            try
            {
                ExpTest2();
            }
            catch
            {
            }

            return true;
        }

        private static bool ExpTest2()
        {
            Foo(200, null);
            throw new Exception();

            return false;
        }
    }
}
