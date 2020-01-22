using NUnit.Framework;
using SqlQueryStressEngine;
using SqlQueryStressEngine.Parameters;
using SqlQueryStressEngine.Tests.Unit.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlQueryStress.Tests.Unit
{
    public class QueryStressTestTests
    {
        private const string _query = "SELECT 1";
        private const string _connectionString = "SERVER=BLAH";

        private QueryStressTest _queryStressTest;

        [SetUp]
        public void Setup()
        {
            FakeQueryWorker.ClearTotalExecutions();
            FakeQueryWorker.ClearInstances();

            _queryStressTest = new QueryStressTest
            {
                ConnectionString = _connectionString,
                ThreadCount = 1,
                Iterations = 1,
                DbProvider = new FakeDbProvider(),
                Query = _query,
                OnQueryExecutionComplete = (_) => { },
                QueryParameters = Array.Empty<ParameterSet>()
            };
        }

        [Test]
        public void GivenSingleThreadedTestWithOneIteration_QueryIsExecutedOnce()
        {
            _queryStressTest.BeginInvoke();
            _queryStressTest.Wait();

            Assert.AreEqual(1, FakeQueryWorker.TotalExecutionCount);
        }

        [TestCase(2, 10)]
        [TestCase(2, 1)]
        public void GivenMultiThreadedTestWithMultipleIterations_QueryIsExecutedNTimesFromEachThread(int threads, int iterations)
        {
            _queryStressTest.ThreadCount = threads;
            _queryStressTest.Iterations = iterations;

            _queryStressTest.BeginInvoke();
            _queryStressTest.Wait();

            Assert.AreEqual(threads, FakeQueryWorker.Instances.Count);
            Assert.IsTrue(FakeQueryWorker.Instances.All(x => x.ExecutionCount == iterations));
        }
    }
}