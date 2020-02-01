using SqlQueryStressEngine;
using SqlQueryStressEngine.Parameters;
using System;
using System.Collections.Generic;

namespace SqlQueryStressGUI.TestEnvironment
{
    public class QueryExecutionDetailsViewModel : ViewModel
    {
        private QueryExecution _queryExecution;
        public QueryExecution QueryExecution
        {
            get => _queryExecution;
            set
            {
                SetProperty(value, ref _queryExecution);

                NotifyPropertyChanged(nameof(ExecutionStatisticsTable));
                NotifyPropertyChanged(nameof(QueryParameters));
                NotifyPropertyChanged(nameof(ExecutionError));
            }
        }

        public QueryExecutionStatisticsTable ExecutionStatisticsTable
        {
            get
            {
                if(QueryExecution == null)
                {
                    return new QueryExecutionStatisticsTable();
                }
                else
                {
                    return QueryExecutionStatisticsTable.CreateFromExecutionResult(QueryExecution);
                }
            }
        }

        public IEnumerable<ParameterValue> QueryParameters
        {
            get => QueryExecution.Parameters?.Parameters ?? Array.Empty<ParameterValue>();
        }

        public string ExecutionError
        {
            get => QueryExecution.ExecutionError?.Message ?? "N/A";
        }
    }
}
