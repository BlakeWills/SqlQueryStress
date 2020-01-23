using System;

namespace SqlQueryStressGUI
{
    public class DatabaseConnection
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public DbProvider DbProvider { get; set; }
    }
}
