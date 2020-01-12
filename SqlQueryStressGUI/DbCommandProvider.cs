using SqlQueryStressEngineGUI;
using System.Collections.Generic;

namespace SqlQueryStressGUI
{
    public sealed class DbCommandProvider
    {
        private readonly DbProviderFactory _dbProviderFactory;

        public DbCommandProvider(DbProviderFactory dbProviderFactory)
        {
            _dbProviderFactory = dbProviderFactory;
        }

        public IEnumerable<DbCommand> GetDbCommands(DbProvider dbProvider)
        {
            var provider = _dbProviderFactory.GetDbProvider(dbProvider);

            foreach(var command in provider.GetDbCommands())
            {
                yield return new DbCommand
                {
                    Name = command.Name,
                    Command = (connection) => command.Invoke(connection.ConnectionString)
                };
            }
        }
    }
}
