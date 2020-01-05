using System;
using System.Collections.Generic;
using System.Threading;

namespace SqlQueryStressEngine
{
    public class QueryStressTest
    {
        private IEnumerable<Thread> _threads;

        public IDbProvider DbProvider { get; set; }

        public int ThreadCount { get; set; }

        public int Iterations { get; set; }

        public string Query { get; set; }

        public string ConnectionString { get; set; }

        public Action<QueryExecutionStatistics> OnQueryExecutionComplete { get; set; }

        public IEnumerable<KeyValuePair<string, object>> QueryParameters { get; set; }

        public void BeginInvoke()
        {
            DbProvider.BeforeTestStart();
            QueryWorkerParameters workerParameters = GetQueryWorkerParameters();

            var threads = new Thread[ThreadCount];

            for (int i = 0; i < ThreadCount; i++)
            {
                var worker = DbProvider.GetQueryWorker();
                var thread = new Thread(() => worker.Start(workerParameters, OnQueryExecutionComplete));
                threads[i] = thread;
                thread.Start();
            }

            _threads = threads;
        }

        public void Wait()
        {
            if (_threads == null)
            {
                return;
            }

            foreach (var t in _threads)
            {
                t.Join();
            }
        }

        private QueryWorkerParameters GetQueryWorkerParameters()
        {
            return new QueryWorkerParameters()
            {
                Iterations = Iterations,
                ConnectionString = ConnectionString,
                Query = Query,
                QueryParameters = QueryParameters
            };
        }
    }
}
