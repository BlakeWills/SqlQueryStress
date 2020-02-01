using SqlQueryStressEngine;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SqlQueryStressGUI.TestEnvironment
{
    public class QueryExecutionStatisticsTable : DataTable
    {
        private PropertyInfo[] PropertyInfos { get; set; }

        private readonly Dictionary<DataRow, QueryExecution> _queryExecutionMap;

        public QueryExecutionStatisticsTable()
        {
            _queryExecutionMap = new Dictionary<DataRow, QueryExecution>();
        }

        public static QueryExecutionStatisticsTable CreateFromExecutionResult(QueryExecution queryExecution)
        {
            var props = queryExecution.GetType()
                .GetProperties()
                .Where(pi => pi.Name != nameof(queryExecution.Parameters))
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

            _queryExecutionMap.Add(row, queryExecution);

            Rows.Add(row);
        }

        public QueryExecution GetExecution(DataRow row) => _queryExecutionMap[row];
    }
}
