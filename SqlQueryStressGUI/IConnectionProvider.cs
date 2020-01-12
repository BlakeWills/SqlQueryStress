using System.Collections.Generic;
using System;

namespace SqlQueryStressGUI
{
    public interface IConnectionProvider
    {
        IEnumerable<DatabaseConnection> GetConnections();

        void SaveConnection(DatabaseConnection connection);

        void DeleteConnection(DatabaseConnection connection);

        event EventHandler<ConnectionsChangedEventArgs> ConnectionsChanged;
    }

    public class ConnectionsChangedEventArgs
    {
        public ConnectionsChangedEventArgs(IEnumerable<DatabaseConnection> connections)
        {
            Connections = connections;
        }

        public IEnumerable<DatabaseConnection> Connections { get; }
    }
}