using SqlQueryStressEngine;
using System;

namespace SqlQueryStress.DbProviders.Couchbase
{
    public class CouchbaseQueryWorker : IQueryWorker
    {
        public void Start(QueryWorkerParameters workerParameters, Action<QueryExecutionStatistics> onQueryExecutionComplete)
        {
            throw new NotImplementedException();
        }
    }
}
