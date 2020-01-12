using System.Collections.Generic;

namespace SqlQueryStressEngine
{
    public interface IDbProvider
    {
        void BeforeTestStart();

        IQueryWorker GetQueryWorker();

        IEnumerable<IDbCommand> GetDbCommands();
    }
}
