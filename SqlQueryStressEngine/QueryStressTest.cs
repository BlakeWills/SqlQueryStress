using System.Collections.Generic;
using System.Threading;

namespace SqlQueryStressEngine
{
    public class QueryStressTest<TDbProvider> : IQueryStressTest<TDbProvider> where TDbProvider : IQueryWorker
    {
        private IEnumerable<Thread> _threads;
        private readonly QueryWorkerFactory _queryWorkerFactory;

        public QueryStressTest(QueryStressTestParameters stressTestParameters)
        {
            TestParameters = stressTestParameters;
            _queryWorkerFactory = new QueryWorkerFactory();
        }

        public QueryStressTestParameters TestParameters { get; }

        public void BeginInvoke()
        {
            var workers = _queryWorkerFactory.GetQueryWorkers<TDbProvider>(TestParameters.ThreadCount);
            var workerParameters = QueryWorkerParameters.Build(TestParameters);

            var threads = new Thread[TestParameters.ThreadCount];

            int workerIndex = 0;
            foreach (var worker in workers)
            {
                var thread = new Thread(() => worker.Start(workerParameters, TestParameters.OnQueryExecutionComplete));
                threads[workerIndex] = thread;
                thread.Start();
                workerIndex++;
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
    }
}
