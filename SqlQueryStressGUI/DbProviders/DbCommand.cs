using SqlQueryStressGUI.DbProviders;
using System;

namespace SqlQueryStressGUI.DbProviders
{
    public class DbCommand
    {
        public string Name { get; set; }

        public Action<DatabaseConnection> Command { get; set; }
    }
}
