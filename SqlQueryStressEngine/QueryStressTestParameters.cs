using System;
using System.Collections.Generic;

namespace SqlQueryStressEngine
{
    public class QueryStressTestParameters
    {
        public QueryStressTestParameters(
            int threadCount,
            int iterations,
            string query,
            string connectionString,
            Action<QueryExecutionStatistics> onQueryExecutionComplete,
            IEnumerable<KeyValuePair<string, object>> queryParameters)
        {
            ThreadCount = threadCount;
            Iterations = iterations;
            Query = query;
            ConnectionString = connectionString;
            OnQueryExecutionComplete = onQueryExecutionComplete;
            QueryParameters = queryParameters;
        }

        public int ThreadCount { get; }

        public int Iterations { get; }

        public string Query { get; }

        public string ConnectionString { get; }

        public Action<QueryExecutionStatistics> OnQueryExecutionComplete { get; }

        public IEnumerable<KeyValuePair<string, object>> QueryParameters { get; }
    }
}
