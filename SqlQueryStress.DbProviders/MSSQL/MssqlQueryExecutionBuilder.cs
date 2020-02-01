using SqlQueryStressEngine.Parameters;

namespace SqlQueryStress.DbProviders.MSSQL
{
    internal class MssqlQueryExecutionBuilder
    {
        public double ElapsedMilliseconds { get; set; }

        public double CpuMilliseconds { get; set; }

        public int LogicalReads { get; set; }

        public double ClientElapsedMilliseconds { get; set; }

        public ParameterSet QueryParameters { get; set; }

        public MssqlQueryExecution Build() => new MssqlQueryExecution(ElapsedMilliseconds, CpuMilliseconds, LogicalReads, ClientElapsedMilliseconds)
        {
            Parameters = QueryParameters
        };
    }
}
