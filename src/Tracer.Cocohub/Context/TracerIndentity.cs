using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer.Cocohub.Context
{
    public class TracerIndentity
    {
        public static TracerIndentity Create()
        {
            var tracer = new TracerIndentity();
            tracer._tracerId = Guid.NewGuid().ToString("N");
            tracer._spanId = "0";


            return tracer;
        }

        public static TracerIndentity Create(string tracerId)
        {
            var tracer = new TracerIndentity();
            Guid _tracerId;
            if (Guid.TryParse(tracerId, out _tracerId))
                tracer._tracerId = _tracerId.ToString("N");
            else
                tracer._tracerId = Guid.NewGuid().ToString("N");
            tracer._spanId = "0";

            return tracer;
        }

        private TracerIndentity() { }

        public void Enter()
        {
            if (_spanId.Length > _lastSpanId.Length)
            {
                _lastSpanId = _spanId;
                _spanId += ".1";
            }
            else if (_spanId.Length < _lastSpanId.Length)
            {
                _lastSpanId = _spanId;
                string broFuncSpanNum = _lastSpanId.Substring(_lastSpanId.LastIndexOf(".") + 1);
                int cntFuncSpanNum = Convert.ToInt32(broFuncSpanNum) + 1;
                _spanId = _spanId + "." + cntFuncSpanNum.ToString();
            }
            else
            {
                //剩下_spanId.Length == _lastSpanId.Length的情况, 理论上不会出现
                throw new Exception();
            }
        }

        public void Leave()
        {
            _lastSpanId = _spanId;
            _spanId = _spanId.Substring(0, _spanId.LastIndexOf("."));
        }

        private string _tracerId = string.Empty;
        public string TracerId { get { return _tracerId; } }

        private string _lastSpanId = string.Empty;
        private string _spanId = string.Empty;
        public string SpanId { get { return _spanId; } }


    }
}
