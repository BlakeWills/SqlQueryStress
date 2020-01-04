﻿using SqlQueryStressEngine;
using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SqlQueryStress.DbProviders.MSSQL
{
    public class MssqlQueryWorker : IQueryWorker
    {
        MssqlMessageParser _messageParser;

        public MssqlQueryWorker()
        {
            _messageParser = new MssqlMessageParser();
        }

        public void Start(
            QueryWorkerParameters workerParameters,
            Action<QueryExecutionStatistics> onQueryExecutionComplete)
        {
            var query = $"SET STATISTICS IO ON;\nSET STATISTICS TIME ON;\n{workerParameters.Query}";

            for (int i = 0; i < workerParameters.Iterations; i++)
            {
                var builder = new MssqlQueryExecutionBuilder();

                using var con = new SqlConnection(workerParameters.ConnectionString);
                using var cmd = new SqlCommand(query, con);

                con.Open();

                con.InfoMessage += (sender, args) =>
                {
                    var result = _messageParser.ParseSqlInfoMessage(args);
                    builder.CpuMilliseconds = result.CpuTime;
                    builder.ElapsedMilliseconds = result.ElapsedTime;
                    builder.LogicalReads = result.LogicalReads;
                };

                foreach (var param in workerParameters.QueryParameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

                var sw = Stopwatch.StartNew();
                cmd.ExecuteNonQuery();
                sw.Stop();

                builder.ClientElapsedMilliseconds = sw.Elapsed.TotalMilliseconds;

                onQueryExecutionComplete(builder.Build());
            }
        }
    }
}
