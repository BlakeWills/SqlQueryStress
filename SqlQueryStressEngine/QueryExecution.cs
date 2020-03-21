using SqlQueryStressEngine.Parameters;
using System;

namespace SqlQueryStressEngine
{
    public abstract class QueryExecution
    {
        public abstract double ElapsedMilliseconds { get; }

        public ParameterSet Parameters { get; set; }

        public Exception ExecutionError { get; set; }

        public string ExecutionPlan { get; set; }
    }
}
