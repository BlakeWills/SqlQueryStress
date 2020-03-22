using NUnit.Framework;
using SqlQueryStress.DbProviders.MSSQL;
using SqlQueryStress.DbProviders.Tests.QueryPlans;

namespace SqlQueryStress.DbProviders.Tests
{
    [TestFixture]
    public class MssqlExecutionPlanParserTests
    {
        private MssqlExecutionPlanParser _executionPlanParser;

        [SetUp]
        public void TestSetup()
        {
            _executionPlanParser = new MssqlExecutionPlanParser();
        }

        [TestCaseSource(typeof(ExecutionPlanTestCaseProvider), nameof(ExecutionPlanTestCaseProvider.TestCases))]
        public void ReturnsCorrectCpuTime(ExecutionPlanTestCase testCase)
        {
            var result = _executionPlanParser.Parse(testCase.ExecutionPlan);

            Assert.AreEqual(testCase.CpuTime, result.CpuTime);
        }

        [TestCaseSource(typeof(ExecutionPlanTestCaseProvider), nameof(ExecutionPlanTestCaseProvider.TestCases))]
        public void ReturnsCorrectElapsedTime(ExecutionPlanTestCase testCase)
        {
            var result = _executionPlanParser.Parse(testCase.ExecutionPlan);

            Assert.AreEqual(testCase.ElapsedTime, result.ElapsedTime);
        }

        [TestCaseSource(typeof(ExecutionPlanTestCaseProvider), nameof(ExecutionPlanTestCaseProvider.TestCases))]
        public void ReturnsCorrectLogicalReads(ExecutionPlanTestCase testCase)
        {
            var result = _executionPlanParser.Parse(testCase.ExecutionPlan);

            Assert.AreEqual(testCase.LogicalReads, result.LogicalReads);
        }

        [TestCaseSource(typeof(ExecutionPlanTestCaseProvider), nameof(ExecutionPlanTestCaseProvider.TestCases))]
        public void ReturnsCorrectQueryPlanHash(ExecutionPlanTestCase testCase)
        {
            var result = _executionPlanParser.Parse(testCase.ExecutionPlan);

            Assert.AreEqual(testCase.QueryPlanHash, result.QueryPlanHash);
        }
    }
}
