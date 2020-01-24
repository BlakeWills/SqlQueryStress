using SqlQueryStress.DbProviders.MSSQL;
using SqlQueryStressEngine;
using System;

namespace SqlQueryStressGUI.DbProviders
{
    public sealed class DbProviderFactory
    {
        public IDbProvider GetDbProvider(DbProvider dbProvider) => dbProvider switch
        {
            DbProvider.MSSQL => new MssqlDbProvider(),
            _ => throw new ArgumentException()
        };
    }
}
