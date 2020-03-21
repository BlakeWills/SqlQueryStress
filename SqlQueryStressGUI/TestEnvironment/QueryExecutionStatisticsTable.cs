using SqlQueryStressEngine;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SqlQueryStressGUI.TestEnvironment
{
    public class QueryExecutionStatisticsTable : DataTable
    {
        public const string _errorColumnHeader = "Errors";

        private PropertyInfo[] PropertyInfos { get; set; }
        private readonly Dictionary<DataRow, QueryExecution> _queryExecutionMap;

        private static readonly string[] _queryExecutionIgnoredPropertyNames =
            new[] { nameof(QueryExecution.Parameters), nameof(QueryExecution.ExecutionError), nameof(QueryExecution.ExecutionPlan) };

        public QueryExecutionStatisticsTable()
        {
            _queryExecutionMap = new Dictionary<DataRow, QueryExecution>();
        }

        public static QueryExecutionStatisticsTable CreateFromExecutionResult(QueryExecution queryExecution)
        {
            var props = queryExecution.GetType()
                .GetProperties()
                .Where(pi => !_queryExecutionIgnoredPropertyNames.Contains(pi.Name))
                .ToArray();

            var dataTable = new QueryExecutionStatisticsTable()
            {
                PropertyInfos = props
            };

            foreach (var prop in props)
            {
                dataTable.Columns.Add(new DataColumn(prop.Name, prop.PropertyType)
                {
                    ReadOnly = true
                });
            }

            dataTable.Columns.Add(new DataColumn(_errorColumnHeader, typeof(bool))
            {
                ReadOnly = true
            });

            dataTable.AddRow(queryExecution);

            return dataTable;
        }

        public void AddRow(QueryExecution queryExecution)
        {
            var row = NewRow();

            foreach (var prop in PropertyInfos)
            {
                row[prop.Name] = prop.GetValue(queryExecution);
            }

            row[_errorColumnHeader] = queryExecution.ExecutionError != null;

            _queryExecutionMap.Add(row, queryExecution);

            Rows.Add(row);
        }

        public QueryExecution GetExecution(DataRow row) => _queryExecutionMap[row];
    }
}
