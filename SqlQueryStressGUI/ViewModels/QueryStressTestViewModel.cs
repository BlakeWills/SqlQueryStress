using SqlQueryStressEngine;
using SqlQueryStressEngine.DbProviders.MSSQL;
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

            QueryExecutionResults = new ObservableCollection<QueryExecutionStatistics>();
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

        private ObservableCollection<QueryExecutionStatistics> _queryExecutionResults;
        public ObservableCollection<QueryExecutionStatistics> QueryExecutionResults
        {
            get => _queryExecutionResults;
            set => SetProperty(value, ref _queryExecutionResults);
        }

        private void StartQueryStressTest(object queryText)
        {
            // Will need to switch here dependant on selected query builder.
            var test = new QueryStressTestBuilder<MssqlQueryWorker>().BuildQueryStressTest(
                ThreadCount,
                Iterations,
                (string)queryText,
                ConnectionString,
                new KeyValuePair<string, object>[0],
                onQueryExecutionComplete: (result) => {
                    Application.Current.Dispatcher.BeginInvoke(() => QueryExecutionResults.Add(result));
                });

            test.BeginInvoke();
        }
    }
}
