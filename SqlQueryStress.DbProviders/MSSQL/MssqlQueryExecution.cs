using SqlQueryStressEngine;

namespace SqlQueryStress.DbProviders.MSSQL
{
    public sealed class MssqlQueryExecution : QueryExecution
    {
        public MssqlQueryExecution(
            double elapsedMilliseconds,
            double cpuMilliseconds,
            int logicalReads,
            double clientElapsedMilliseconds,
            string planXml)
        {
            ElapsedMilliseconds = elapsedMilliseconds;
            CpuMilliseconds = cpuMilliseconds;
            LogicalReads = logicalReads;
            ClientElapsedMilliseconds = clientElapsedMilliseconds;
            ExecutionPlan = planXml;
        }

        public override double ElapsedMilliseconds { get; }

        public double CpuMilliseconds { get; }

        public int LogicalReads { get; }

        public double ClientElapsedMilliseconds { get; }
    }
}
