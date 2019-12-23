using NUnit.Framework;
using SqlQueryStressEngine;
using SqlQueryStressEngine.Tests.Unit.Fakes;
using System.Collections.Generic;
using System.Threading;

namespace SqlQueryStress.Tests.Unit
{
    public class ProgressReportingTests
    {
        private const string _query = "SELECT 1";
        private const string _connectionString = "SERVER=BLAH";

        [SetUp]
        public void Setup()
        {

        }

        // Is this a good test? We're essentially testing a fake thing.
        // I think it's fine to keep in whilst we're still deciding the API - but this isn't actually testing anything.
        [Test]
        public void QueryStatisticsAreReportedAfterEachExecution()
        {
            int onQueryExecutionInvocations = 0;
            int iterations = 5;

            var queryStressTest = new QueryStressTestBuilder<FakeQueryWorker>().BuildQueryStressTest(
                threadCount: 1,
                iterations,
                query: _query,
                connectionString: _connectionString,
                queryParameters: new KeyValuePair<string, object>[0],
                (executionStats) => {
                    Interlocked.Increment(ref onQueryExecutionInvocations);
                });

            queryStressTest.BeginInvoke();

            queryStressTest.Wait();

            Assert.AreEqual(iterations, FakeQueryWorker.TotalExecutionCount);
        }
    }
}