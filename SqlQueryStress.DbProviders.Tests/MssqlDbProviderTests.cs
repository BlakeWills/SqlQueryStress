using NUnit.Framework;
using SqlQueryStress.DbProviders.MSSQL;

namespace SqlQueryStress.DbProviders.Tests
{
    public class MssqlDbProviderTests
    {
        [Test]
        public void GetQueryWorker_ReturnsInstanceOfMssqlQueryWorker()
        {
            Assert.IsInstanceOf<MssqlQueryWorker>(new MssqlDbProvider().GetQueryWorker());
        }
    }
}