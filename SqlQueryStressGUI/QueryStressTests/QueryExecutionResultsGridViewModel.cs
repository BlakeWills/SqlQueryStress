using SqlQueryStressEngine;
using System.Collections.ObjectModel;

namespace SqlQueryStressGUI.QueryStressTests
{
    public class QueryExecutionResultsGridViewModel : ViewModel
    {
        public ObservableCollection<QueryExecutionStatistics> Results { get; set; }
    }
}
