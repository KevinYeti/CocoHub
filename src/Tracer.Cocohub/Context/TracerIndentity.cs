using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tracer.Cocohub.Context
{
    public class TracerIndentity
    {
        public static TracerIndentity Create()
        {
            var tracer = new TracerIndentity();
            //generate a tracerid
            //guid(length:32) + random(length:8)
            tracer._tracerId = Guid.NewGuid().ToString("N") + Rnd(10000000, 99999999).ToString();

            tracer._spanId = "0";

            return tracer;
        }

        private static TracerIndentity Create(string tracerId, string spanId)
        {
            var tracer = new TracerIndentity();

            tracer._tracerId = tracerId;
 
            tracer._spanId = spanId;

            return tracer;
        }

        public static string[] Spliter = new string[] { ":" };

        public static TracerIndentity FromString(string s)
        {
            if (s.Contains(":"))
            {
                var splits = s.Split(Spliter, StringSplitOptions.RemoveEmptyEntries );
                return TracerIndentity.Create(splits[0], splits[1]);
            }
            else
                return null;
        }

        private TracerIndentity() { }

        public override string ToString()
        {
            string s = string.Empty;
            s = string.Format("{0}:{1}", _tracerId, _spanId);

            return s;
        }

        public void Enter()
        {
            if (_spanId.Length > _lastSpanId.Length)
            {
                _lastSpanId = _spanId;
                _spanId += ".1";
            }
            else if (_spanId.Length < _lastSpanId.Length)
            {
                string broFuncSpanNum = _lastSpanId.Substring(_lastSpanId.LastIndexOf(".") + 1);
                int cntFuncSpanNum = Convert.ToInt32(broFuncSpanNum) + 1;
                _lastSpanId = _spanId;
                _spanId = _spanId + "." + cntFuncSpanNum.ToString();
            }
            else
            {
                //剩下_spanId.Length == _lastSpanId.Length的情况, 理论上不会出现
                Console.WriteLine(string.Format("SpanId[{0}] error with wrong _lastSpanId[{1}].", _spanId, _lastSpanId));
            }
        }

        public void Leave()
        {
            try
            {
                _lastSpanId = _spanId;
                _spanId = _spanId.Substring(0, _spanId.LastIndexOf("."));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unexpected exception:" + ex.Message);
            }
        }

        public string EnterAsync()
        {
            //计算逻辑原理基本和Enter一致,但不记录_lastSpanId,仅去获得Enter后应得的spanId
            string _asyncSpanId = _spanId;
            if (_spanId.Length > _lastSpanId.Length)
            {
                _asyncSpanId += ".1";
            }
            else if (_spanId.Length < _lastSpanId.Length)
            {
                string broFuncSpanNum = _asyncSpanId.Substring(_asyncSpanId.LastIndexOf(".") + 1);
                int cntFuncSpanNum = Convert.ToInt32(broFuncSpanNum) + 1;
                _asyncSpanId = _asyncSpanId + "." + cntFuncSpanNum.ToString();
            }
            else
            {
                //剩下_spanId.Length == _lastSpanId.Length的情况, 理论上不会出现
                Console.WriteLine(string.Format("Async:SpanId[{0}] error with wrong _lastSpanId[{1}].", _spanId, _lastSpanId));
            }

            string s = string.Empty;
            s = string.Format("{0}:{1}", _tracerId, _asyncSpanId);

            return s;
        }

        private string _tracerId = string.Empty;
        public string TracerId { get { return _tracerId; } }

        private string _lastSpanId = string.Empty;
        private string _spanId = string.Empty;
        public string SpanId { get { return _spanId; } }

        private static int Rnd(int MinValue, int MaxValue)
        {
            byte[] array = new byte[1];
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            rNGCryptoServiceProvider.GetNonZeroBytes(array);
            int num = Convert.ToInt32(array[0]);
            System.Random random = new System.Random(DateTime.Now.Millisecond * num);
            return random.Next(MinValue, MaxValue);
        }
    }
}
