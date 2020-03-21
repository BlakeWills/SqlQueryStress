using SqlQueryStressEngine.Parameters;
using System.Threading;

namespace SqlQueryStressEngine
{
    public class QueryWorkerParameters
    {
        public int Iterations { get; set; }

        public string ConnectionString { get; set; }

        public string Query { get; set; }

        public ParameterSet QueryParameters { get; set; }

        public CancellationToken CancellationToken { get; set; }
    }
}
