using SqlQueryStressEngine;
using System.Data.SqlClient;

namespace SqlQueryStress.DbProviders.MSSQL
{
    public sealed class FreeProcCacheDbCommand : IDbCommand
    {
        private const string _command = "DBCC FREEPROCCACHE";

        public string Name { get => _command; }

        public void Invoke(string connectionString)
        {
            using var con = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(_command, con);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
