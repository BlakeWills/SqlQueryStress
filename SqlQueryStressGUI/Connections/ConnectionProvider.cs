using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SqlQueryStressGUI.Connections
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionDirectory;

        public ConnectionProvider()
        {
            _connectionDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "SQLQueryStressGUI",
                "Connections");

            if(!Directory.Exists(_connectionDirectory))
            {
                Directory.CreateDirectory(_connectionDirectory);
            }
        }

        public event EventHandler<ConnectionsChangedEventArgs> ConnectionsChanged;

        public IEnumerable<DatabaseConnection> GetConnections()
        {
            var connectionFiles = Directory.EnumerateFiles(_connectionDirectory);

            var connections = new List<DatabaseConnection>();
            foreach (var conFile in connectionFiles)
            {
                connections.Add(JsonConvert.DeserializeObject<DatabaseConnection>(File.ReadAllText(conFile)));
            }

            return connections;
        }

        public void SaveConnection(DatabaseConnection connection)
        {
            if (connection.Id == Guid.Empty)
            {
                connection.Id = Guid.NewGuid();
            }

            string path = GetFilePath(connection);
            var contents = JsonConvert.SerializeObject(connection);
            File.WriteAllText(path, contents);

            ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs(GetConnections()));
        }

        public void DeleteConnection(DatabaseConnection connection)
        {
            File.Delete(GetFilePath(connection));
            ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs(GetConnections()));
        }

        private string GetFilePath(DatabaseConnection connection)
        {
            return Path.Combine(_connectionDirectory, $"{connection.Id}.txt");
        }
    }
}
