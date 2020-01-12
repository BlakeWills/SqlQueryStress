using Microsoft.Extensions.DependencyInjection;
using SqlQueryStress.DbProviders.MSSQL;
using SqlQueryStressEngine;
using SqlQueryStressEngineGUI;
using SqlQueryStressGUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SqlQueryStressGUI.ViewModels
{
    public sealed class QueryStressTestViewModel : ViewModel
    {
        private const string _connectionManagerText = "Connection Manager...";
        private readonly IConnectionProvider _connectionProvider;

        public QueryStressTestViewModel(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _connectionProvider.ConnectionsChanged += (sender, args) =>
            {
                Connections = BuildConnectionList(args.Connections);
            };

            Connections = BuildConnectionList(_connectionProvider.GetConnections());

            SelectedConnection = Connections.First();

            GoCommandHandler = new CommandHandler(StartQueryStressTest);
            ConnectionChangedCommand = new CommandHandler((_) => OnConnectionChanged());
        }

        public CommandHandler GoCommandHandler { get; set; }

        public CommandHandler ConnectionChangedCommand { get; set; }

        private ObservableCollection<DatabaseConnection> _connections;
        public ObservableCollection<DatabaseConnection> Connections
        {
            get => _connections;
            set => SetProperty(value, ref _connections);
        }

        private DatabaseConnection _selectedConnection;
        public DatabaseConnection SelectedConnection
        {
            get => _selectedConnection;
            set => SetProperty(value, ref _selectedConnection);
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

        private TimeSpan _avgExecutionTime;
        public TimeSpan AverageExecutionTime
        {
            get => _avgExecutionTime;
            set => SetProperty(value, ref _avgExecutionTime);
        }

        public List<QueryExecutionStatistics> Results { get; private set; }

        private void StartQueryStressTest(object queryText)
        {
            Results = new List<QueryExecutionStatistics>();
            QueryExecutionStatisticsTable?.Clear();

            var timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 25)
            };

            var test = BuildQueryStressTest(queryText.ToString());

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

        private QueryStressTest BuildQueryStressTest(string queryText) => new QueryStressTest
        {
            DbProvider = GetDbProvider(SelectedConnection.DbProvider),
            ThreadCount = ThreadCount,
            Iterations = Iterations,
            Query = queryText,
            ConnectionString = SelectedConnection.ConnectionString,
            QueryParameters = Array.Empty<KeyValuePair<string, object>>(),
            OnQueryExecutionComplete = (result) =>
            {
                Application.Current.Dispatcher.BeginInvoke(() => AddQueryExecutionResult(result));
            }
        };

        private IDbProvider GetDbProvider(DbProvider dbProvider) => dbProvider switch
        {
            DbProvider.MSSQL => new MssqlDbProvider(),
            _ => throw new ArgumentException()
        };

        private void AddQueryExecutionResult(QueryExecutionStatistics executionStatistics)
        {
            Results.Add(executionStatistics);

            AverageExecutionTime = TimeSpan.FromMilliseconds(Results.Average(x => x.ElapsedMilliseconds));

            // This is thread safe as we only ever call it from the UI thread.
            if (QueryExecutionStatisticsTable == null)
            {
                QueryExecutionStatisticsTable = QueryExecutionStatisticsTable.CreateFromExecutionResult(executionStatistics);
            }
            else
            {
                QueryExecutionStatisticsTable.AddRow(executionStatistics);
            }
        }

        private void OnConnectionChanged()
        {
            if(SelectedConnection.Name == _connectionManagerText)
            {
                OpenConnectionManager();
            }
        }

        private void OpenConnectionManager()
        {
            SelectedConnection = default;

            var conManager = DiContainer.Instance.ServiceProvider.GetRequiredService<ConnectionManager>();
            conManager.ShowDialog();
        }

        private ObservableCollection<DatabaseConnection> BuildConnectionList(IEnumerable<DatabaseConnection> connections)
        {
            return new ObservableCollection<DatabaseConnection>(connections.Append(new DatabaseConnection { Name = _connectionManagerText }));
        }
    }
}
