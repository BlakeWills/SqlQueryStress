using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SqlQueryStressGUI.ViewModels
{
    public class ConnectionManagerViewModel : ViewModel
    {
        private readonly IConnectionProvider _connectionProvider;

        public ConnectionManagerViewModel(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _connectionProvider.ConnectionsChanged += ConnectionProvider_ConnectionsChanged;

            var connectionVms = _connectionProvider.GetConnections().Select(x => BuildConnectionViewModel(x));

            Connections = new ObservableCollection<AddEditConnectionViewModel>(connectionVms);

            NewConnectionCommand = new CommandHandler((_) => ShowConnectionWindow());
            EditConnectionCommand = new CommandHandler((con) => ShowConnectionWindow((AddEditConnectionViewModel)con));
            DeleteConnectionCommand = new CommandHandler((con) => DeleteConnection((AddEditConnectionViewModel)con));

            CloseCommand = new CommandHandler((closable) => ((ICloseable)closable).Close());
        }

        public CommandHandler NewConnectionCommand { get; }

        public CommandHandler EditConnectionCommand { get; }

        public CommandHandler DeleteConnectionCommand { get; }

        public CommandHandler CloseCommand { get; }

        private ObservableCollection<AddEditConnectionViewModel> _connections;
        public ObservableCollection<AddEditConnectionViewModel> Connections
        {
            get => _connections;
            set => SetProperty(value, ref _connections);
        }

        private void ShowConnectionWindow(AddEditConnectionViewModel connectionViewModel = null)
        {
            var addEditWindow = DiContainer
                .Instance
                .ServiceProvider
                .GetRequiredService<ConnectionWindowFactory>()
                .Build(connectionViewModel);

            addEditWindow.ShowDialog();
        }

        private void ConnectionProvider_ConnectionsChanged(object sender, ConnectionsChangedEventArgs e)
        {
            Connections = new ObservableCollection<AddEditConnectionViewModel>(e.Connections.Select(x => BuildConnectionViewModel(x)));
        }

        private AddEditConnectionViewModel BuildConnectionViewModel(DatabaseConnection connection)
        {
            return new AddEditConnectionViewModel(_connectionProvider)
            {
                Id = connection.Id,
                Name = connection.Name,
                ConnectionString = connection.ConnectionString,
                DbProvider = connection.DbProvider.ToString()
            };
        }

        private void DeleteConnection(AddEditConnectionViewModel connectionViewModel)
        {
            _connectionProvider.DeleteConnection(connectionViewModel.ToDatabaseConnection());
        }
    }
}
