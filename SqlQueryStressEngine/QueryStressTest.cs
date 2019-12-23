using System;
using System.Collections.Generic;
using System.Threading;

namespace SqlQueryStressEngine
{
    public class QueryStressTest<TDbProvider> where TDbProvider : IQueryWorker
    {
        private readonly QueryWorkerFactory _queryWorkerFactory;

        public QueryStressTest()
        {
            _queryWorkerFactory = new QueryWorkerFactory();
        }

        public int ThreadCount { get; internal set; }

        public int Iterations { get; internal set; }

        public string Query { get; internal set; }

        public string ConnectionString { get; internal set; }

        public Action<QueryExecutionStatistics> OnQueryExecutionComplete { get; internal set; }

        public IEnumerable<KeyValuePair<string, object>> QueryParameters { get; internal set; }

        private IEnumerable<Thread> Threads { get; set; }

        public void BeginInvoke()
        {
            var workers = _queryWorkerFactory.GetQueryWorkers<TDbProvider>(ThreadCount);

            var workerParameters = new QueryWorkerParameters
            {
                Iterations = Iterations,
                ConnectionString = ConnectionString,
                Query = Query,
                QueryParameters = QueryParameters
            };

            var threads = new Thread[ThreadCount];

            int workerIndex = 0;
            foreach (var worker in workers)
            {
                var thread = new Thread(() => worker.Start(workerParameters, OnQueryExecutionComplete));
                threads[workerIndex] = thread;
                thread.Start();
                workerIndex++;
            }

            Threads = threads;
        }

        public void Wait()
        {
            if (Threads == null)
            {
                return;
            }

            foreach(var t in Threads)
            {
                t.Join();
            }
        }
    }
}
