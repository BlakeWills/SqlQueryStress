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
        private CancellationTokenSource _cancellationTokenSource;

        public IDbProvider DbProvider { get; set; }

        public int ThreadCount { get; set; }

        public int Iterations { get; set; }

        public string Query { get; set; }

        public string ConnectionString { get; set; }

        public Action<QueryExecution> OnQueryExecutionComplete { get; set; }

        public IEnumerable<ParameterSet> QueryParameters { get; set; }

        public QueryStressTestState State { get; private set; } = QueryStressTestState.NotStarted;

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

            _cancellationTokenSource = new CancellationTokenSource();
            
            _threads = GetWorkerThreads(_cancellationTokenSource.Token);
            _stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < ThreadCount; i++)
            {
                _threads[i].Start();
            }

            State = QueryStressTestState.Executing;
        }

        private Thread[] GetWorkerThreads(CancellationToken cancellationToken)
        {
            var threads = new Thread[ThreadCount];

            for (int i = 0; i < ThreadCount; i++)
            {
                var worker = DbProvider.GetQueryWorker();
                var workerParameters = GetQueryWorkerParameters(i, cancellationToken);

                var thread = new Thread(() =>
                {
                    worker.Start(workerParameters, OnQueryExecutionComplete);

                    var completedThreads = Interlocked.Increment(ref _completeThreads);
                    if (completedThreads == ThreadCount)
                    {
                        _stopwatch.Stop();

                        State = workerParameters.CancellationToken.IsCancellationRequested
                            ? QueryStressTestState.Stopped
                            : QueryStressTestState.Complete;

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

        private QueryWorkerParameters GetQueryWorkerParameters(int workerId, CancellationToken cancellationToken)
        {
            var parameters = QueryParameters.Any() ? QueryParameters.ElementAt(workerId) : new ParameterSet();

            return new QueryWorkerParameters()
            {
                Iterations = Iterations,
                ConnectionString = ConnectionString,
                Query = Query,
                QueryParameters = parameters,
                CancellationToken = cancellationToken
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

        public void RequestCancellation()
        {
            if(IsRunning)
            {
                _cancellationTokenSource.Cancel();
                State = QueryStressTestState.Stopping;
            }
            else
            {
                throw new InvalidOperationException("Cannot cancel a test that is not running.");
            }
        }
    }
}
