namespace SqlQueryStress.DbProviders.MSSQL
{
    internal sealed class MssqlExecutionPlanStats
    {
        public double CpuTime { get; set; }

        public double ElapsedTime { get; set; }

        public int LogicalReads { get; set; }

        public string QueryPlanHash { get; set; }
    }
}
