using SqlQueryStressEngine.Parameters;
using System;

namespace SqlQueryStress.DbProviders.MSSQL
{
    internal class MssqlQueryExecutionBuilder
    {
        public double ElapsedMilliseconds { get; set; }

        public double CpuMilliseconds { get; set; }

        public int LogicalReads { get; set; }

        public double ClientElapsedMilliseconds { get; set; }

        public ParameterSet QueryParameters { get; set; }

        public Exception ExecutionError { get; set; }

        public MssqlQueryExecution Build() => new MssqlQueryExecution(ElapsedMilliseconds, CpuMilliseconds, LogicalReads, ClientElapsedMilliseconds)
        {
            Parameters = QueryParameters,
            ExecutionError = ExecutionError
        };
    }
}
