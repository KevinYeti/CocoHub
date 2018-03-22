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
            _copyHelper = new PostgreSQLCopyHelper<LogEntity>("public", "Log")
                .MapTimeStamp("LogTime", x => x.LogTime)
                .MapText("Level", x => x.Level)
                .MapText("Method", x => x.Method)
                .MapText("Action", x => x.Action)
                .MapText("Params", x => x.Params)
                .MapInteger("Time", x => x.Time)
                .MapText("SpanId", x => x.SpanId)
                .MapText("TraceId", x => x.TraceId)
                .MapBit("HasException", x => x.HasException)
                .MapText("Result", x => x.Result)
                .MapText("IP", x => x.IP);
        }

        public static void WriteToDatabase(IEnumerable<LogEntity> entities)
        {
            using (var connection = new NpgsqlConnection("Server=10.99.66.86;Port=5432;Database=postgres;User Id=postgres;Password=123456;"))
            {
                connection.Open();
                _copyHelper.SaveAll(connection, entities);
            }
        }
    }
}
