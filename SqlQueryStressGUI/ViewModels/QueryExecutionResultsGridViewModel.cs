using SqlQueryStressEngine;
using System.Collections.ObjectModel;

namespace SqlQueryStressGUI.ViewModels
{
    public class QueryExecutionResultsGridViewModel : ViewModel
    {
        public ObservableCollection<QueryExecutionStatistics> Results { get; set; }
    }
}
