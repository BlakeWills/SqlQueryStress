using SqlQueryStressEngine.Parameters;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SqlQueryStress.DbProviders.MSSQL
{
    public class MssqlParameterValueBuilder : IParameterValueBuilder
    {
        private readonly string _parameterName;
        private readonly string _connectionString;
        private readonly string _parameterQuery;

        private object[] _values;
        private int _valueCounter;

        public MssqlParameterValueBuilder(
            string parameterName,
            string connectionString,
            string parameterQuery)
        {
            _parameterName = parameterName;
            _connectionString = connectionString;
            _parameterQuery = parameterQuery;
        }

        public ParameterValue GetNextValue()
        {
            if(_values == null)
            {
                _values = GetValues();
            }

            return new ParameterValue(_parameterName, _values[_valueCounter++ % _values.Length]);
        }

        private object[] GetValues()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(_parameterQuery, connection);

            connection.Open();

            var reader = command.ExecuteReader();
            var values = new List<object>();
            while(reader.Read())
            {
                values.Add(reader.GetValue(0));
            }

            return values.ToArray();
        }
    }
}
