using Agent.Cocohub.Entity;
using System;

namespace Agent.Cocohub.Log
{
    public static class LogResolver
    {
        public static bool TryResolve(string log, out LogEntity entity)
        {
            entity = null;
            if (string.IsNullOrEmpty(log))
                return false;

            //here is a sample data 
            //[2018-04-10 20:43:30.419] [Info] {"Action":"Return","Method":"System.String[] LifeVC.Biz.OrderLogic.OrdersBiz::GetRegion(System.Int32)","Result":"$return=System.String[]","Time":25,"TracerId":"718f1d9f02054b40a68ce241f4fa98e310643936","SpanId":"0.2.1.3"}
            string[] parts = new string[3];
            parts[0] = log.Substring(1, 23).Trim();    //2018-04-10 20:43:30.419
            string sub = log.Substring(27);
            parts[1] = sub.Substring(0, sub.IndexOf("]")).Trim();  //Info
            parts[2] = log.Substring(log.IndexOf("{")).Trim();      //{json}

            try
            {
                entity = DynamicJson.Deserialize<LogEntity>(parts[2]);

                entity.LogTime = DateTime.Parse(parts[0]);

                entity.Level = parts[1];

                entity.HasException = parts[2].Contains("$exception");
            }
            catch(Exception ex)
            {
                entity = null;
                return false;
            }

            return true;
        }
    }
}
