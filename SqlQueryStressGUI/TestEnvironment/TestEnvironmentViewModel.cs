using Microsoft.Extensions.DependencyInjection;
using SqlQueryStressGUI.DbProviders;
using SqlQueryStressGUI.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SqlQueryStressGUI.TestEnvironment
{
    public class TestEnvironmentViewModel : ViewModel
    {
        private const string _connectionManagerText = "Connection Manager...";
        private readonly IConnectionProvider _connectionProvider;
        private readonly DbCommandProvider _dbCommandProvider;
        private readonly ParameterViewModelBuilder _parameterViewModelBuilder;
        private readonly IViewFactory _viewFactory;

        private readonly int _processorCount = Environment.ProcessorCount;
        private readonly int _defaultIterations = 10;

        public TestEnvironmentViewModel(
            IViewFactory viewFactory,
            IConnectionProvider connectionProvider,
            DbCommandProvider dbCommandProvider,
            ParameterViewModelBuilder parameterViewModelBuilder)
        {
            _viewFactory = viewFactory;
            _connectionProvider = connectionProvider;
            _dbCommandProvider = dbCommandProvider;
            _parameterViewModelBuilder = parameterViewModelBuilder;
            _connectionProvider.ConnectionsChanged += (sender, args) =>
            {
                Connections = BuildConnectionList(args.Connections);
                ActiveTest.SelectedConnection = Connections.First();
                ExecuteCommandHandler?.RaiseCanExecuteChanged();
            };

            Connections = BuildConnectionList(_connectionProvider.GetConnections());
            Tests = new ObservableCollection<QueryStressTestViewModel>();
            
            AddNewQueryStressTest();
            OnConnectionChanged();

            ExecuteCommandHandler = new CommandHandler((_) => Execute());
            NewQueryStressTestCommandHandler = new CommandHandler((_) => AddNewQueryStressTest());
            ConnectionDropdownClosedCommand = new CommandHandler((_) => OnConnectionDropdownClosed());
            ConnectionChangedCommand = new CommandHandler((_) => OnConnectionChanged());
            DbCommandSelected = new CommandHandler((dbCommand) => InvokeDbCommand((DbCommand)dbCommand));
            OpenParameterSettingsCommand = new CommandHandler((_) => OpenParameterSettings());
            RemoveTestCommand = new CommandHandler((test) => RemoveTest((QueryStressTestViewModel)test));
        }

        public CommandHandler ExecuteCommandHandler { get; }

        public CommandHandler NewQueryStressTestCommandHandler { get; }

        public CommandHandler ConnectionDropdownClosedCommand { get; }

        public CommandHandler ConnectionChangedCommand { get; }

        public CommandHandler DbCommandSelected { get; }

        public CommandHandler OpenParameterSettingsCommand { get; }

        public CommandHandler RemoveTestCommand { get; }

        private QueryStressTestViewModel _activeTest;
        public QueryStressTestViewModel ActiveTest
        {
            get => _activeTest;
            set => SetProperty(value, ref _activeTest);
        }

        private ObservableCollection<QueryStressTestViewModel> _tests;
        public ObservableCollection<QueryStressTestViewModel> Tests
        {
            get => _tests;
            set => SetProperty(value, ref _tests);
        }

        private ObservableCollection<DatabaseConnection> _connections;
        public ObservableCollection<DatabaseConnection> Connections
        {
            get => _connections;
            set => SetProperty(value, ref _connections);
        }

        private IEnumerable<DbCommand> _dbCommands;
        public IEnumerable<DbCommand> DbCommands
        {
            get => _dbCommands;
            set => SetProperty(value, ref _dbCommands);
        }

        public void Execute()
        {
            var validationResult = ValidateActiveTest();

            if(validationResult.IsTestValid())
            {
                ActiveTest.StartQueryStressTest();
            }
            else
            {
                var invalidTestViewModel = new InvalidQueryStressTestViewModel() { ValidationResult = validationResult };
                _viewFactory.ShowDialog(invalidTestViewModel);
            }
        }

        private ObservableCollection<DatabaseConnection> BuildConnectionList(IEnumerable<DatabaseConnection> connections)
        {
            return new ObservableCollection<DatabaseConnection>(connections.Append(new DatabaseConnection
            {
                Name = _connectionManagerText,
                DbProvider = DbProvider.NotSpecified
            }));
        }

        private QueryStressTestValidationResult ValidateActiveTest()
        {
            return new QueryStressTestValidationResult
            {
                ThreadCountValid = ActiveTest.ThreadCount > 0,
                IterationsValid = ActiveTest.Iterations > 0,
                QueryValid = !string.IsNullOrWhiteSpace(ActiveTest.Query),
                QueryParametersValid = IsQueryParametersValid(),
                ConnectionValid = IsConnectionValid()
            };
        }

        private bool IsQueryParametersValid()
        {
            var hasParams = _parameterViewModelBuilder.QueryHasParameters(ActiveTest.Query);
            return !hasParams || (hasParams && ActiveTest.QueryParameters.Any());
        }

        private bool IsConnectionValid()
        {
            return ActiveTest.SelectedConnection != null && ActiveTest.SelectedConnection.DbProvider != DbProvider.NotSpecified;
        }

        private void AddNewQueryStressTest()
        {
            var newTest = DiContainer.Instance.ServiceProvider.GetRequiredService<QueryStressTestViewModel>();
            
            newTest.SelectedConnection = Connections.First();
            newTest.ThreadCount = _processorCount - 1; // -1 for the UI thread.
            newTest.Iterations = _defaultIterations;
            
            Tests.Add(newTest);
            ActiveTest = newTest;
        }

        private void OnConnectionDropdownClosed()
        {
            if (ActiveTest.SelectedConnection.Name == _connectionManagerText)
            {
                ActiveTest.SelectedConnection = Connections.First();

                _viewFactory.ShowDialog<ConnectionManagerViewModel>();
            }
        }

        private void OnConnectionChanged()
        {
            if (IsConnectionValid())
            {
                DbCommands = _dbCommandProvider.GetDbCommands(ActiveTest.SelectedConnection.DbProvider);
            }
            else
            {
                DbCommands = Array.Empty<DbCommand>();
            }
        }

        private void InvokeDbCommand(DbCommand command)
        {
            command.Command(ActiveTest.SelectedConnection);
        }

        private void OpenParameterSettings()
        {
            UpdateQueryParameters();

            var managerViewModel = new ParameterManagerViewModel(ActiveTest.QueryParameters, _viewFactory);
            _viewFactory.ShowDialog(managerViewModel);

            void UpdateQueryParameters()
            {
                var queryParams = ActiveTest.QueryParameters.ToList();
                _parameterViewModelBuilder.UpdateQueryParameterViewModels(ActiveTest.Query, ref queryParams);
                ActiveTest.QueryParameters = queryParams;
            }
        }

        private void RemoveTest(QueryStressTestViewModel test)
        {
            Tests.Remove(test);

            if(!Tests.Any())
            {
                AddNewQueryStressTest();
            }

            if (ActiveTest == test)
            {
                ActiveTest = Tests.First();
            }
        }
    }
}
