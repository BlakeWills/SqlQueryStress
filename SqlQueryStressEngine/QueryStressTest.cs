using SqlQueryStressEngine.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public Action<QueryExecution> OnQueryExecutionComplete { get; set; }

        public IEnumerable<ParameterSet> QueryParameters { get; set; }

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
            
            _threads = GetWorkerThreads();
            _stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < ThreadCount; i++)
            {
                _threads[i].Start();
            }
        }

        private Thread[] GetWorkerThreads()
        {
            var threads = new Thread[ThreadCount];

            for (int i = 0; i < ThreadCount; i++)
            {
                var worker = DbProvider.GetQueryWorker();
                var workerParameters = GetQueryWorkerParameters(i);

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

        private QueryWorkerParameters GetQueryWorkerParameters(int workerId)
        {
            var parameters = QueryParameters.Any() ? QueryParameters.ElementAt(workerId) : new ParameterSet();

            return new QueryWorkerParameters()
            {
                Iterations = Iterations,
                ConnectionString = ConnectionString,
                Query = Query,
                QueryParameters = parameters
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
