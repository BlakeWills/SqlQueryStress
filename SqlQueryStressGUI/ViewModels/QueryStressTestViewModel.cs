using SqlQueryStress.DbProviders.MSSQL;
using SqlQueryStressEngine;
using SqlQueryStressEngineGUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace SqlQueryStressGUI.ViewModels
{
    public sealed class QueryStressTestViewModel : ViewModel
    {
        public QueryStressTestViewModel()
        {
            AvailableDbProviders = Enum.GetNames(typeof(DbProvider));
            GoCommandHandler = new CommandHandler(StartQueryStressTest);
        }

        public CommandHandler GoCommandHandler { get; set; }

        public IEnumerable<string> AvailableDbProviders { get; }

        private DbProvider _selectedDbProvider;
        public DbProvider SelectedDbProvider
        {
            get => _selectedDbProvider;
            set => SetProperty(value, ref _selectedDbProvider);
        }

        private string _connectionString;
        public string ConnectionString
        {
            get => _connectionString;
            set => SetProperty(value, ref _connectionString);
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

        private QueryExecutionStatisticsTable _queryExecutionStatisticsTable;
        public QueryExecutionStatisticsTable QueryExecutionStatisticsTable
        {
            get => _queryExecutionStatisticsTable;
            set => SetProperty(value, ref _queryExecutionStatisticsTable);
        }

        private TimeSpan _elapsed;
        public TimeSpan Elapsed
        {
            get => _elapsed;
            set => SetProperty(value, ref _elapsed);
        }

        private void StartQueryStressTest(object queryText)
        {
            var timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 100)
            };

            var test = new QueryStressTest
            {
                DbProvider = GetDbProvider(SelectedDbProvider),
                ThreadCount = ThreadCount,
                Iterations = Iterations,
                Query = queryText.ToString(),
                ConnectionString = ConnectionString,
                OnQueryExecutionComplete = (result) =>
                {
                    Application.Current.Dispatcher.BeginInvoke(() => AddQueryExecutionResult(result));
                },
                QueryParameters = Array.Empty<KeyValuePair<string, object>>(),
            };

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

        private IDbProvider GetDbProvider(DbProvider dbProvider) => dbProvider switch
        {
            DbProvider.MSSQL => new MssqlDbProvider(),
            _ => throw new ArgumentException()
        };

        private void AddQueryExecutionResult(QueryExecutionStatistics executionStatistics)
        {
            if (QueryExecutionStatisticsTable == null)
            {
                QueryExecutionStatisticsTable = QueryExecutionStatisticsTable.CreateFromExecutionResult(executionStatistics);
            }
            else
            {
                QueryExecutionStatisticsTable.AddRow(executionStatistics);
            }
        }
    }
}
