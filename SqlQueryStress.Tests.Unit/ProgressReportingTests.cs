﻿using NUnit.Framework;
using SqlQueryStressEngine;
using SqlQueryStressEngine.Tests.Unit.Fakes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SqlQueryStress.Tests.Unit
{
    public class ProgressReportingTests
    {
        private const string _query = "SELECT 1";
        private const string _connectionString = "SERVER=BLAH";

        // There are two aspects to progress reporting that need to be tested:
        // 1. The domain layer (QueryStressTest) handles the action the client passes it correctly. This test suite covers that.
        //    This test suite also ensures we do not break the public facing API.
        // 2. The ports (IQueryWorker implementations) invoke the action after each query execution, with the correct stats. 
        //    This is not tested here as it is IQueryWorker implementations are not part of the core engine / domain layer.

        [Test]
        public void QueryStatisticsAreReportedAfterEachExecution()
        {
            int onQueryExecutionInvocations = 0;
            int iterations = 5;

            var testParams = new QueryStressTestParameters(
               threadCount: 1,
               iterations: iterations,
               query: _query,
               connectionString: _connectionString,
               queryParameters: Array.Empty<KeyValuePair<string, object>>(),
               onQueryExecutionComplete: (executionStats) => {
                   Interlocked.Increment(ref onQueryExecutionInvocations);
               });

            var queryStressTest = new QueryStressTest<FakeQueryWorker>(testParams);

            queryStressTest.BeginInvoke();
            queryStressTest.Wait();

            Assert.AreEqual(iterations, FakeQueryWorker.TotalExecutionCount);
        }
    }
}