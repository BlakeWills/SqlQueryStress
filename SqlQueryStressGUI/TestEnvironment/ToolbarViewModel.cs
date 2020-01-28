using SqlQueryStressGUI.DbProviders;
using SqlQueryStressGUI.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SqlQueryStressGUI.TestEnvironment
{
    public class ToolbarViewModel : ViewModel
    {
        private const string _connectionManagerText = "Connection Manager...";
        private readonly IConnectionProvider _connectionProvider;
        private readonly DbCommandProvider _dbCommandProvider;
        private readonly ParameterViewModelBuilder _parameterViewModelBuilder;
        private readonly IViewFactory _viewFactory;

        public ToolbarViewModel(
            IConnectionProvider connectionProvider,
            DbCommandProvider dbCommandProvider,
            ParameterViewModelBuilder parameterViewModelBuilder,
            IViewFactory viewFactory)
        {
            _connectionProvider = connectionProvider;
            _dbCommandProvider = dbCommandProvider;
            _parameterViewModelBuilder = parameterViewModelBuilder;
            _viewFactory = viewFactory;

            _connectionProvider.ConnectionsChanged += (sender, args) =>
            {
                Connections = BuildConnectionList(args.Connections);
                SelectedConnection = Connections.First();
            };

            Connections = BuildConnectionList(_connectionProvider.GetConnections());
            SelectedConnection = Connections.First();
            OnConnectionChanged();

            QueryParameters = new List<ParameterViewModel>();

            GoCommandHandler = new CommandHandler((_) => RaiseOnExecute(), canExecute: (_) => IsConnectionValid());
            ConnectionDropdownClosedCommand = new CommandHandler((_) => OnConnectionDropdownClosed());
            ConnectionChangedCommand = new CommandHandler((_) => OnConnectionChanged());
            DbCommandSelected = new CommandHandler((dbCommand) => InvokeDbCommand((DbCommand)dbCommand));
            ParameterSettingsCommand = new CommandHandler((query) => OpenParameterSettings((string)query));
        }

        public CommandHandler GoCommandHandler { get; }

        public CommandHandler ConnectionChangedCommand { get; }

        public CommandHandler ConnectionDropdownClosedCommand { get; }

        public CommandHandler DbCommandSelected { get; }

        public CommandHandler ParameterSettingsCommand { get; set; }

        public event EventHandler OnExecute;

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
            set
            {
                SetProperty(value, ref _selectedConnection);
                RaiseCanExecuteChanged();
            }
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

        private IEnumerable<DbCommand> _dbCommands;
        public IEnumerable<DbCommand> DbCommands
        {
            get => _dbCommands;
            set => SetProperty(value, ref _dbCommands);
        }

        private List<ParameterViewModel> _queryParameters;
        public List<ParameterViewModel> QueryParameters
        {
            get => _queryParameters;
            set => SetProperty(value, ref _queryParameters);
        }

        private void RaiseOnExecute()
        {
            OnExecute?.Invoke(this, EventArgs.Empty);
        }

        private void OnConnectionDropdownClosed()
        {
            if (SelectedConnection.Name == _connectionManagerText)
            {
                SelectedConnection = Connections.First();

                _viewFactory.ShowDialog<ConnectionManagerViewModel>();
            }
        }

        private void OnConnectionChanged()
        {
            if (IsConnectionValid())
            {
                DbCommands = _dbCommandProvider.GetDbCommands(SelectedConnection.DbProvider);
            }
            else
            {
                DbCommands = Array.Empty<DbCommand>();
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

        private void InvokeDbCommand(DbCommand command)
        {
            command.Command(SelectedConnection);
        }

        private bool IsConnectionValid()
        {
            return SelectedConnection != null && SelectedConnection.DbProvider != DbProvider.NotSpecified;
        }

        private void RaiseCanExecuteChanged()
        {
            GoCommandHandler?.RaiseCanExecuteChanged();
        }

        private void OpenParameterSettings(string query)
        {
            UpdateQueryParameters(query);

            var managerViewModel = new ParameterManagerViewModel(QueryParameters, _viewFactory);
            _viewFactory.ShowDialog(managerViewModel);
        }

        private void UpdateQueryParameters(string query)
        {
            var queryParams = QueryParameters.ToList();
            _parameterViewModelBuilder.UpdateQueryParameterViewModels(query, ref queryParams);
            QueryParameters = queryParams;
        }
    }
}
