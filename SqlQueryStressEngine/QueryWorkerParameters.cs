using System.Collections.Generic;

namespace SqlQueryStressEngine
{
    public class QueryWorkerParameters
    {
        public int Iterations { get; set; }

        public string ConnectionString { get; set; }

        public string Query { get; set; }

        public IEnumerable<KeyValuePair<string, object>> QueryParameters { get; set; }
    }
}
