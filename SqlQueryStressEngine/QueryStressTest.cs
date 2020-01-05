using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SqlQueryStressEngine
{
    public class QueryStressTest
    {
        private Thread[] _threads;
        private int _completeThreads = 0;
        private Stopwatch _stopwatch;
        private readonly object _mutex = new object();

        public IDbProvider DbProvider { get; set; }

        public int ThreadCount { get; set; }

        public int Iterations { get; set; }

        public string Query { get; set; }

        public string ConnectionString { get; set; }

        public Action<QueryExecutionStatistics> OnQueryExecutionComplete { get; set; }

        public IEnumerable<KeyValuePair<string, object>> QueryParameters { get; set; }

        public event EventHandler StressTestComplete;

        public TimeSpan Elapsed
        {
            get
            {
                if (_stopwatch == null)
                {
                    return new TimeSpan();
                }
                else
                {
                    return _stopwatch.Elapsed;
                }
            }
        }

        public bool IsRunning { get; private set; }

        public void BeginInvoke()
        {
            SetIsRunning();

            DbProvider.BeforeTestStart();

            var workerParameters = GetQueryWorkerParameters();
            _threads = GetWorkerThreads(workerParameters);

            _stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < ThreadCount; i++)
            {
                _threads[i].Start();
            }
        }

        private Thread[] GetWorkerThreads(QueryWorkerParameters workerParameters)
        {
            var threads = new Thread[ThreadCount];

            for (int i = 0; i < ThreadCount; i++)
            {
                var worker = DbProvider.GetQueryWorker();
                var thread = new Thread(() =>
                {
                    worker.Start(workerParameters, OnQueryExecutionComplete);

                    var completedThreads = Interlocked.Increment(ref _completeThreads);
                    if (completedThreads == ThreadCount)
                    {
                        _stopwatch.Stop();
                        StressTestComplete?.Invoke(this, new EventArgs());
                    }
                });

                threads[i] = thread;
            }

            return threads;
        }

        private void SetIsRunning()
        {
            lock (_mutex)
            {
                if (IsRunning)
                {
                    throw new InvalidOperationException("Test already running");
                }
                else
                {
                    IsRunning = true;
                }
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
