using NUnit.Framework;
using System.Collections;

namespace SqlQueryStress.DbProviders.Tests.QueryPlans
{
    internal class ExecutionPlanTestCaseProvider
    {
        private static readonly ExecutionPlanTestCase _monthlyProductSales =
            new ExecutionPlanTestCase(QueryPlanResources.AdventureWorks_MonthlyProductSales, cpuTime: 48, elapsedTime: 173, logicalReads: 1235, queryPlanHash: "0x6B5AA6FAEEE95BC0");

        private static readonly ExecutionPlanTestCase _salesPersonByProduct =
            new ExecutionPlanTestCase(QueryPlanResources.AdventureWorks_SalesPersonByProduct, cpuTime: 8, elapsedTime: 8, logicalReads: 122, queryPlanHash: "0x6ECFDF3DCF9DF5CF");

        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(_monthlyProductSales);
                yield return new TestCaseData(_salesPersonByProduct);
            }
        }
    }

    public class ExecutionPlanTestCase
    {
        public ExecutionPlanTestCase(
            string executionPlan,
            double cpuTime,
            double elapsedTime,
            int logicalReads,
            string queryPlanHash)
        {
            ExecutionPlan = executionPlan;
            CpuTime = cpuTime;
            ElapsedTime = elapsedTime;
            LogicalReads = logicalReads;
            QueryPlanHash = queryPlanHash;
        }

        public string ExecutionPlan { get; }

        public double CpuTime { get; }

        public double ElapsedTime { get; }

        public int LogicalReads { get; }

        public string QueryPlanHash { get; }
    }
}
