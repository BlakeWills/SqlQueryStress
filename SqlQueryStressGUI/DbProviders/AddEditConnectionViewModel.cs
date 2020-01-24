using System;
using System.Collections.Generic;

namespace SqlQueryStressGUI.DbProviders
{
    public class AddEditConnectionViewModel : ViewModel
    {
        private readonly IConnectionProvider _connectionProvider;

        public AddEditConnectionViewModel(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;

            AvailableDbProviders = Enum.GetNames(typeof(DbProvider));
            SaveAndCloseCommand = new CommandHandler((closeable) => SaveAndClose((ICloseable)closeable));
        }

        public CommandHandler SaveAndCloseCommand { get; private set; }

        public IEnumerable<string> AvailableDbProviders { get; }

        public Guid Id { get; set; }

        private string _dbProvider;
        public string DbProvider
        {
            get => _dbProvider;
            set => SetProperty(value, ref _dbProvider);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(value, ref _name);
        }

        private string _connectionString;

        public string ConnectionString
        {
            get => _connectionString;
            set => SetProperty(value, ref _connectionString);
        }

        private void SaveAndClose(ICloseable closeable)
        {
            Save();
            closeable.Close();
        }

        private void Save()
        {
            _connectionProvider.SaveConnection(ToDatabaseConnection());
        }

        public DatabaseConnection ToDatabaseConnection()
        {
            return new DatabaseConnection
            {
                Id = Id,
                Name = Name,
                ConnectionString = ConnectionString,
                DbProvider = (DbProvider)Enum.Parse(typeof(DbProvider), DbProvider)
            };
        }
    }
}
