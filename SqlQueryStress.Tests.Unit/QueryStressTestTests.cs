using NUnit.Framework;
using SqlQueryStressEngine;
using SqlQueryStressEngine.Tests.Unit.Fakes;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SqlQueryStress.Tests.Unit
{
    public class QueryStressTestTests
    {
        private const string _query = "SELECT 1";
        private const string _connectionString = "SERVER=BLAH";

        [SetUp]
        public void Setup()
        {
            FakeQueryWorker.ClearTotalExecutions();
            FakeQueryWorker.ClearInstances();
        }

        [Test]
        public void GivenSingleThreadedTestWithOneIteration_QueryIsExecutedOnce()
        {
            var queryStressTest = new QueryStressTestBuilder<FakeQueryWorker>().BuildQueryStressTest(
                threadCount: 1,
                iterations: 1,
                query: _query,
                connectionString: _connectionString,
                queryParameters: new KeyValuePair<string, object>[0],
                onQueryExecutionComplete: (_) => { });

            queryStressTest.BeginInvoke();

            queryStressTest.Wait();

            Assert.AreEqual(1, FakeQueryWorker.TotalExecutionCount);
        }

        [TestCase(2, 10)]
        [TestCase(2, 1)]
        public void GivenMultiThreadedTestWithMultipleIterations_QueryIsExecutedNTimesFromEachThread(int threads, int iterations)
        {
            var queryStressTest = new QueryStressTestBuilder<FakeQueryWorker>().BuildQueryStressTest(
                threads,
                iterations,
                _query,
                _connectionString,
                queryParameters: new KeyValuePair<string, object>[0],
                onQueryExecutionComplete: (_) => { });

            queryStressTest.BeginInvoke();

            queryStressTest.Wait();

            Assert.AreEqual(threads, FakeQueryWorker.Instances.Count);
            Assert.IsTrue(FakeQueryWorker.Instances.All(x => x.ExecutionCount == iterations));
        }
    }
}