using SqlQueryStressEngine;

namespace SqlQueryStress.DbProviders.MSSQL
{
    public sealed class MssqlQueryExecution : QueryExecutionStatistics
    {
        public MssqlQueryExecution(
            double elapsedMilliseconds,
            double cpuMilliseconds,
            int logicalReads,
            double clientElapsedMilliseconds)
        {
            ElapsedMilliseconds = elapsedMilliseconds;
            CpuMilliseconds = cpuMilliseconds;
            LogicalReads = logicalReads;
            ClientElapsedMilliseconds = clientElapsedMilliseconds;
        }

        public override double ElapsedMilliseconds { get; }

        public double CpuMilliseconds { get; }

        public int LogicalReads { get; }

        public double ClientElapsedMilliseconds { get; }
    }
}
