using System;
using System.Collections.Generic;

namespace SqlQueryStressEngine
{
    public class QueryStressTestBuilder<TDbProvider> where TDbProvider : IQueryWorker
    {
        public QueryStressTest<TDbProvider> BuildQueryStressTest(
            int threadCount,
            int iterations,
            string query,
            string connectionString,
            IEnumerable<KeyValuePair<string, object>> queryParameters,
            Action<QueryExecutionStatistics> onQueryExecutionComplete)
        {
            return new QueryStressTest<TDbProvider>
            {
                ConnectionString = connectionString,
                Iterations = iterations,
                ThreadCount = threadCount,
                Query = query,
                QueryParameters = queryParameters,
                OnQueryExecutionComplete = onQueryExecutionComplete
            };
        }
    }
}
