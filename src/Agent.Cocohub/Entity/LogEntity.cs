using System;
namespace Agent.Cocohub.Entity
{
    public class LogEntity
    {
        public string Method { get; set; }
        public string Params { get; set; }
        public string Action { get; set; }
        public decimal Time { get; set; }
        public DateTime LogTime { get; set; }
        public string Level { get; set; }
        public string SpanId { get; set; }
        public string TracerId { get; set; }
        public bool HasException { get; set; }
        public string Result { get; set; }
        public string IP { get; set; }

    }
}
