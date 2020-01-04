using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SqlQueryStressEngine.Tests.Unit.Fakes
{
    internal class FakeQueryWorker : IQueryWorker
    {
        private int _executionCount;
        public int ExecutionCount
        {
            get => _executionCount;
        }

        private static int _totalExecutionCount;
        public static int TotalExecutionCount
        {
            get => _totalExecutionCount;
        }

        public FakeQueryWorker()
        {
            Instances.Add(this);
        }

        static FakeQueryWorker()
        {
            Instances = new ConcurrentBag<FakeQueryWorker>();
        }

        public static ConcurrentBag<FakeQueryWorker> Instances { get; private set; }

        public static void ClearTotalExecutions() => _totalExecutionCount = 0;

        public static void ClearInstances() => Instances = new ConcurrentBag<FakeQueryWorker>();

        public void Start(QueryWorkerParameters workerParameters, Action<QueryExecutionStatistics> onQueryExecutionComplete)
        {
            Interlocked.Add(ref _executionCount, workerParameters.Iterations);
            Interlocked.Add(ref _totalExecutionCount, workerParameters.Iterations);

            onQueryExecutionComplete(new FakeQueryExecutionStatistics());
        }
    }

    public class FakeQueryExecutionStatistics : QueryExecutionStatistics
    {
        public override double ElapsedMilliseconds { get; }
    }
}
