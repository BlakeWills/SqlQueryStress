using System;
using System.Data.SqlClient;

namespace SqlQueryStressEngine.DbProviders.MSSQL
{
    public class MssqlQueryWorker : IQueryWorker
    {
        public void Start(
            QueryWorkerParameters workerParameters,
            Action<QueryExecutionStatistics> onQueryExecutionComplete)
        {
            for (int i = 0; i < workerParameters.Iterations; i++)
            {
                using var con = new SqlConnection(workerParameters.ConnectionString);
                using var cmd = new SqlCommand(workerParameters.Query, con);

                con.Open();

                foreach (var param in workerParameters.QueryParameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

                cmd.ExecuteNonQuery();

                // TODO: Read these from somewhere
                var logicalReads = 123;
                var cpuTime = 0.876;
                var executionTime = 1.234;

                onQueryExecutionComplete(new MssqlQueryExecution(executionTime, cpuTime, logicalReads));
            }
        }
    }
}
