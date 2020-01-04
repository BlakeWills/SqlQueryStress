using SqlQueryStressEngine;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for QueryExecutionResultsPanel.xaml
    /// </summary>
    public partial class QueryExecutionResultsPanel : UserControl
    {
        public QueryExecutionResultsPanel()
        {
            InitializeComponent();
        }

        static QueryExecutionResultsPanel()
        {
            //ResultsProperty = DependencyProperty.Register("Results", typeof(ObservableCollection<QueryExecutionStatistics>), typeof(QueryExecutionResultsPanel));
            ResultsProperty = DependencyProperty.Register("Results", typeof(QueryExecutionStatisticsTable), typeof(QueryExecutionResultsPanel));
        }

        //public static readonly DependencyProperty ResultsProperty;

        //public ObservableCollection<QueryExecutionStatistics> Results
        //{
        //    get => (ObservableCollection<QueryExecutionStatistics>)GetValue(ResultsProperty);
        //    set => SetValue(ResultsProperty, value);
        //}


        public static readonly DependencyProperty ResultsProperty;

        public QueryExecutionStatisticsTable Results
        {
            get => (QueryExecutionStatisticsTable)GetValue(ResultsProperty);
            set => SetValue(ResultsProperty, value);
        }
    }
}
