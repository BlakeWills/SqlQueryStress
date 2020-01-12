using System;

namespace SqlQueryStressGUI
{
    public class DbCommand
    {
        public string Name { get; set; }

        public Action<DatabaseConnection> Command { get; set; }
    }
}
