using System;

namespace SqlQueryStressEngine
{
    public interface IDbCommand
    {
        string Name { get; }

        void Invoke(string connectionString);
    }
}
