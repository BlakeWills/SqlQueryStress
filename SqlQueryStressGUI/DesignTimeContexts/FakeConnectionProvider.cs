using SqlQueryStressGUI.DbProviders;
using System;
using System.Collections.Generic;

namespace SqlQueryStressGUI.DesignTimeContexts
{
    public class FakeConnectionProvider : IConnectionProvider
    {
        public event EventHandler<ConnectionsChangedEventArgs> ConnectionsChanged;

        public IEnumerable<DatabaseConnection> GetConnections()
        {
            yield return new DatabaseConnection()
            {
                Name = "SQL01"
            };
        }

        public void SaveConnection(DatabaseConnection connection)
        {
            throw new NotImplementedException();
        }

        public void DeleteConnection(DatabaseConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
