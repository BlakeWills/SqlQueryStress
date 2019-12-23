namespace SqlQueryStressEngine.DbProviders.MSSQL
{
    public sealed class MssqlQueryExecution : QueryExecutionStatistics
    {
        public MssqlQueryExecution(
            double elapsedTime,
            double cpuTime,
            int logicalReads)
        {
            ElapsedTime = elapsedTime;
            CpuTime = cpuTime;
            LogicalReads = logicalReads;
        }

        public override double ElapsedTime { get; }

        public double CpuTime { get; }

        public int LogicalReads { get; }
    }
}
