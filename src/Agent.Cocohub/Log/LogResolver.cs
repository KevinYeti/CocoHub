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

            var parts = log.Split(new string[] { "] " }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                return false;

            try
            {
                entity = DynamicJson.Deserialize<LogEntity>(parts[2].Trim());

                entity.LogTime = DateTime.Parse(parts[0].Trim().Replace("[", string.Empty).Replace("]", string.Empty));

                entity.Level = parts[1].Trim().Replace("[", string.Empty).Replace("]", string.Empty);

                entity.HasException = parts[2].ToLower().Contains("exception");
            }
            catch
            {
                entity = null;
                return false;
            }

            return true;
        }
    }
}
