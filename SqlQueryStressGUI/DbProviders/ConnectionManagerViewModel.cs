using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SqlQueryStressGUI.DbProviders
{
    public class ConnectionManagerViewModel : ViewModel
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly IViewFactory _viewFactory;

        public ConnectionManagerViewModel(
            IConnectionProvider connectionProvider,
            IViewFactory viewFactory)
        {
            _connectionProvider = connectionProvider;
            _viewFactory = viewFactory;

            var connectionVms = GetConnections();

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

        private ObservableCollection<AddEditConnectionViewModel> GetConnections()
        {
            return new ObservableCollection<AddEditConnectionViewModel>(
                _connectionProvider.GetConnections().Select(x => BuildConnectionViewModel(x)));
        }

        private void ShowConnectionWindow(AddEditConnectionViewModel connectionViewModel = null)
        {
            if (connectionViewModel == null)
            {
                connectionViewModel = DiContainer.Instance.ServiceProvider.GetRequiredService<AddEditConnectionViewModel>();
            }

            _viewFactory.ShowDialog(connectionViewModel);

            Connections = GetConnections();
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

            Connections = GetConnections();
        }
    }
}
