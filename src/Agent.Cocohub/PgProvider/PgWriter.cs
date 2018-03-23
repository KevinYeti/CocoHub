using System;
using System.Collections.Generic;
using Agent.Cocohub.Entity;
using Npgsql;
using PostgreSQLCopyHelper;

namespace Agent.Cocohub.PgProvider
{
    public static class PgWriter
    {
        private static PostgreSQLCopyHelper<LogEntity> _copyHelper = null;
        static PgWriter()
        {
            _copyHelper = new PostgreSQLCopyHelper<LogEntity>("public", "log")
                .MapTimeStamp("logtime", x => x.LogTime)
                .MapText("level", x => x.Level)
                .MapText("method", x => x.Method)
                .MapText("action", x => x.Action)
                .MapText("params", x => x.Params)
                .MapNumeric("time", x => x.Time)
                .MapText("spanid", x => x.SpanId)
                .MapText("tracerid", x => x.TracerId)
                .MapBit("hasexception", x => x.HasException)
                .MapText("result", x => x.Result)
                .MapText("ip", x => x.IP);
        }

        public static void Write2Db(IEnumerable<LogEntity> entities)
        {
            if (entities == null)
                return;

            using (var connection = new NpgsqlConnection("Server=10.99.66.86;Port=5432;Database=postgres;User Id=postgres;Password=123456;"))
            {
                connection.Open();
                _copyHelper.SaveAll(connection, entities);
            }
        }
    }
}
