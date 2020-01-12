using SqlQueryStressEngine;
using System.Collections.Generic;

namespace SqlQueryStress.DbProviders.MSSQL
{
    public class MssqlDbProvider : IDbProvider
    {
        public void BeforeTestStart() { }

        public IQueryWorker GetQueryWorker() => new MssqlQueryWorker();

        public IEnumerable<IDbCommand> GetDbCommands() => new IDbCommand[] { new FreeProcCacheDbCommand(), new DropBuffersDbCommand() };
    }
}
