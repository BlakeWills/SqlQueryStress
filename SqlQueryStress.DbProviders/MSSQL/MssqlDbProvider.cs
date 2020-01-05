using SqlQueryStressEngine;

namespace SqlQueryStress.DbProviders.MSSQL
{
    public class MssqlDbProvider : IDbProvider
    {
        public void BeforeTestStart() { }

        public IQueryWorker GetQueryWorker() => new MssqlQueryWorker();
    }
}
