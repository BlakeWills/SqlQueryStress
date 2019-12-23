using System;
using System.Collections.Generic;

namespace SqlQueryStressEngine
{
    internal class QueryWorkerFactory
    {
        public IEnumerable<IQueryWorker> GetQueryWorkers<TDbProvider>(int count) where TDbProvider : IQueryWorker
        {
            var workers = new IQueryWorker[count];

            for (int i = 0; i < count; i++)
            {
                workers[i] = Activator.CreateInstance<TDbProvider>();
            }

            return workers;
        }
    }
}
