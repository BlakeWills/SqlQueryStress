using SqlQueryStressEngine;
using System;

namespace SqlQueryStress.DbProviders.Couchbase
{
    public class CouchbaseQueryWorker : IQueryWorker
    {
        public void Start(QueryWorkerParameters workerParameters, Action<QueryExecution> onQueryExecutionComplete)
        {
            throw new NotImplementedException();
        }
    }
}
