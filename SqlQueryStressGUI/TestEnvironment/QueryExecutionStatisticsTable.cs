using SqlQueryStressEngine;
using System.Data;
using System.Reflection;

namespace SqlQueryStressGUI.TestEnvironment
{
    public class QueryExecutionStatisticsTable : DataTable
    {
        private PropertyInfo[] PropertyInfos { get; set; }

        public static QueryExecutionStatisticsTable CreateFromExecutionResult(QueryExecutionStatistics queryExecutionStatistics)
        {
            var props = queryExecutionStatistics.GetType().GetProperties();

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

            dataTable.AddRow(queryExecutionStatistics);

            return dataTable;
        }

        public void AddRow(QueryExecutionStatistics queryExecutionStatistics)
        {
            var row = NewRow();

            foreach (var prop in PropertyInfos)
            {
                row[prop.Name] = prop.GetValue(queryExecutionStatistics);
            }

            Rows.Add(row);
        }
    }
}
