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

        private QueryExecutionStatisticsTable _queryExecutionStatisticsTable;
        public QueryExecutionStatisticsTable QueryExecutionStatisticsTable
        {
            get => _queryExecutionStatisticsTable;
            set => SetProperty(value, ref _queryExecutionStatisticsTable);
        }

        private void StartQueryStressTest(object queryText)
        {
            // Problem is that obscol isn't covariant.
            // Another problem is that it isn't discoverable via intellisense.
            // Q: Do we need it to be discoverable? I can imagine a situation where people implement custom resultAnalysers for their DbProvider.
            // Describe the problem: 
            // We need a collection of query results but the data contained in the result will differ for each type of DbProvider.
            // Options are:
            //  - a custom class for each type of DbProvider.
            //  - dictionary?
            // don't like dictionary as that has the same problems (not discoverable) - not programming against a type.
            // Is there a bigger issue here? I.E How are we going to create the generic QueryStressTest?

            var testParams = new QueryStressTestParameters
            (
                ThreadCount,
                Iterations,
                queryText.ToString(),
                ConnectionString,
                onQueryExecutionComplete: (result) =>
                {
                    Application.Current.Dispatcher.BeginInvoke(() => AddQueryExecutionResult(result));
                },
                queryParameters: Array.Empty<KeyValuePair<string, object>>()
            );

            var test = GetQueryStressTest(SelectedDbProvider, testParams);

            test.BeginInvoke();
        }

        private IQueryStressTest<IQueryWorker> GetQueryStressTest(DbProvider dbProvider, QueryStressTestParameters testParameters) => dbProvider switch
        {
            DbProvider.MSSQL => new QueryStressTest<MssqlQueryWorker>(testParameters),
            _ => throw new ArgumentException()
        };

        private void AddQueryExecutionResult(QueryExecutionStatistics executionStatistics)
        {
            if(QueryExecutionStatisticsTable == null)
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
