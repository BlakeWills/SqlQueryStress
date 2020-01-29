using SqlQueryStressEngine;
using SqlQueryStressEngine.Parameters;
using SqlQueryStressGUI.DbProviders;
using SqlQueryStressGUI.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SqlQueryStressGUI.TestEnvironment
{
    public sealed class QueryStressTestViewModel : ViewModel
    {
        private readonly DbProviderFactory _dbProviderFactory;

        public QueryStressTestViewModel(DbProviderFactory dbProviderFactory)
        {
            _dbProviderFactory = dbProviderFactory;

            QueryParameters = new List<ParameterViewModel>();
            Results = new ObservableCollection<QueryExecutionStatistics>();
        }

        private DatabaseConnection _selectedConnection;
        public DatabaseConnection SelectedConnection
        {
            get => _selectedConnection;
            set => SetProperty(value, ref _selectedConnection);
        }

        private string _query;
        public string Query
        {
            get => _query;
            set => SetProperty(value, ref _query);
        }

        private int _threadCount;
        public int ThreadCount
        {
            get => _threadCount;
            set => SetProperty(value, ref _threadCount);
        }

        private int _iterations;
        public int Iterations
        {
            get => _iterations;
            set => SetProperty(value, ref _iterations);
        }

        private List<ParameterViewModel> _queryParameters;
        public List<ParameterViewModel> QueryParameters
        {
            get => _queryParameters;
            set => SetProperty(value, ref _queryParameters);
        }

        private TimeSpan _elapsed;
        public TimeSpan Elapsed
        {
            get => _elapsed;
            set => SetProperty(value, ref _elapsed);
        }

        private TimeSpan _avgExecutionTime;
        public TimeSpan AverageExecutionTime
        {
            get => _avgExecutionTime;
            set => SetProperty(value, ref _avgExecutionTime);
        }

        public ObservableCollection<QueryExecutionStatistics> Results { get; }

        public void StartQueryStressTest()
        {
            Results.Clear();

            var timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 25)
            };

            var test = BuildQueryStressTest();

            test.StressTestComplete += (sender, args) =>
            {
                timer.Stop();
                Elapsed = test.Elapsed;
            };

            timer.Tick += (sender, args) =>
            {
                Elapsed = test.Elapsed;
            };

            timer.Start();

            test.BeginInvoke();
        }

        private QueryStressTest BuildQueryStressTest() => new QueryStressTest
        {
            DbProvider = _dbProviderFactory.GetDbProvider(SelectedConnection.DbProvider),
            ThreadCount = ThreadCount,
            Iterations = Iterations,
            Query = Query,
            ConnectionString = SelectedConnection.ConnectionString,
            QueryParameters = GetParameterSets(),
            OnQueryExecutionComplete = (result) =>
            {
                Application.Current.Dispatcher.BeginInvoke(() => AddQueryExecutionResult(result));
            }
        };

        private IEnumerable<ParameterSet> GetParameterSets()
        {
            var paramProvider = new ParameterProvider();
            var executions = ThreadCount * Iterations;
            var paramValueBuilders = QueryParameters.Select(x => x.Settings.GetParameterValueBuilder());

            return paramProvider.GetParameterSets(paramValueBuilders, executions);
        }

        private void AddQueryExecutionResult(QueryExecutionStatistics executionStatistics)
        {
            Results.Add(executionStatistics);
            AverageExecutionTime = TimeSpan.FromMilliseconds(Results.Average(x => x.ElapsedMilliseconds));
        }
    }
}
