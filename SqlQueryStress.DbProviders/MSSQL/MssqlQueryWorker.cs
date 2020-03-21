using SqlQueryStressEngine;
using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SqlQueryStress.DbProviders.MSSQL
{
    public class MssqlQueryWorker : IQueryWorker
    {
        MssqlExecutionPlanParser _executionPlanParser;

        public MssqlQueryWorker()
        {
            _executionPlanParser = new MssqlExecutionPlanParser();
        }

        public void Start(
            QueryWorkerParameters workerParameters,
            Action<QueryExecution> onQueryExecutionComplete)
        {
            var query = $"SET STATISTICS XML ON;\n{workerParameters.Query}";

            var iteration = 0;
            while(iteration < workerParameters.Iterations && !workerParameters.CancellationToken.IsCancellationRequested)
            {
                var builder = new MssqlQueryExecutionBuilder()
                {
                    QueryParameters = workerParameters.QueryParameters
                };

                try
                {
                    using var con = new SqlConnection(workerParameters.ConnectionString);
                    using var cmd = new SqlCommand(query, con);

                    con.Open();

                    foreach (var param in workerParameters.QueryParameters.Parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }

                    var sw = Stopwatch.StartNew();
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Don't include time to read the plan as part of the client execution time
                        sw.Stop();

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                builder.PlanXml = reader.GetString(0);
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(builder.PlanXml))
                    {
                        var executionPlanParseResult = _executionPlanParser.Parse(builder.PlanXml);
                        builder.LogicalReads = executionPlanParseResult.LogicalReads;
                        builder.ElapsedMilliseconds = executionPlanParseResult.ElapsedTime;
                        builder.CpuMilliseconds = executionPlanParseResult.CpuTime;
                    }

                    builder.ClientElapsedMilliseconds = sw.Elapsed.TotalMilliseconds;
                }
                catch(Exception ex)
                {
                    builder.ExecutionError = ex;
                }

                onQueryExecutionComplete(builder.Build());
                iteration++;
            }
        }
    }
}
