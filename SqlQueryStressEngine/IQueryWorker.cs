using System;

namespace SqlQueryStressEngine
{
    public interface IQueryWorker
    {
        void Start(QueryWorkerParameters workerParameters, Action<QueryExecution> onQueryExecutionComplete);
    }
}
