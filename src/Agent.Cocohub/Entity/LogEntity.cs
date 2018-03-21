using System;
namespace Agent.Cocohub.Entity
{
    public class LogEntity
    {
        public string Method { get; set; }
        public string Params { get; set; }
        public string Action { get; set; }
        public int Time { get; set; }
        public DateTime LogTime { get; set; }
        public string Level { get; set; }
        public string SpanId { get; set; }
        public string TraceId { get; set; }

    }
}
