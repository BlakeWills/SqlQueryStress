using SqlQueryStressGUI.DbProviders;
using System;
using System.Collections.Generic;

namespace SqlQueryStressGUI.Tests.Unit
{
    public class ConnectionProviderStub : IConnectionProvider
    {
        public event EventHandler<ConnectionsChangedEventArgs> ConnectionsChanged;

        public void DeleteConnection(DatabaseConnection connection)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DatabaseConnection> GetConnections()
        {
            throw new NotImplementedException();
        }

        public void SaveConnection(DatabaseConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
