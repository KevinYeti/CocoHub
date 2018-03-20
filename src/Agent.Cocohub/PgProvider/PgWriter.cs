using System;
using System.Collections.Generic;
using Agent.Cocohub.Entity;
using Npgsql;
using PostgreSQLCopyHelper;

namespace Agent.Cocohub.PgProvider
{
    public static class PgWriter
    {
        static PgWriter()
        {
            var copyHelper = new PostgreSQLCopyHelper<LogEntity>("sample", "unit_test")
                .MapText("Method", x => x.Method)
                .MapText("Params", x => x.Params)
                .MapText("Action", x => x.Action)
                .MapInteger("Time", x => x.Time)
                .MapDate("LogTime", x => x.LogTime);
        }

        public static void WriteToDatabase(PostgreSQLCopyHelper<LogEntity> copyHelper, IEnumerable<LogEntity> entities)
        {
            using (var connection = new NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=sampledb;User Id=philipp;Password=test_pwd;"))
            {
                connection.Open();

                copyHelper.SaveAll(connection, entities);
            }
        }
    }
}
