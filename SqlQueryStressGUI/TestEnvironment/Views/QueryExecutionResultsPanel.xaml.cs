using SqlQueryStressEngine;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI.TestEnvironment.Views
{
    /// <summary>
    /// Interaction logic for QueryExecutionResultsPanel.xaml
    /// </summary>
    public partial class QueryExecutionResultsPanel : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ResultsProperty;

        public QueryExecutionResultsPanel()
        {
            InitializeComponent();
        }

        static QueryExecutionResultsPanel()
        {
            ResultsProperty = DependencyProperty.Register(
                "Results",
                typeof(ObservableCollection<QueryExecutionStatistics>),
                typeof(QueryExecutionResultsPanel),
                new PropertyMetadata(new PropertyChangedCallback(ResultsChanged)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<QueryExecutionStatistics> Results
        {
            get => (ObservableCollection<QueryExecutionStatistics>)GetValue(ResultsProperty);
            set => SetValue(ResultsProperty, value);
        }

        private QueryExecutionStatisticsTable _resultsTable;
        public QueryExecutionStatisticsTable ResultsTable
        {
            get => _resultsTable;
            set => SetProperty(value, ref _resultsTable);
        }

        private static void ResultsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var resultsPanel = (QueryExecutionResultsPanel)d;
            resultsPanel.ResultsTable = new QueryExecutionStatisticsTable();

            var oldResults = (ObservableCollection<QueryExecutionStatistics>)e.OldValue;
            if (oldResults != null)
            {
                oldResults.CollectionChanged -= resultsPanel.Results_CollectionChanged;
            }

            var results = (ObservableCollection<QueryExecutionStatistics>)e.NewValue;
            results.CollectionChanged += resultsPanel.Results_CollectionChanged;
        }

        private void Results_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Reset)
            {
                ResultsTable.Clear();
            }
            
            if(e.NewItems == null)
            {
                return;
            }

            foreach(var result in e.NewItems)
            {
                var executionResult = (QueryExecutionStatistics)result;

                if (ResultsTable.Rows.Count == 0)
                {
                    ResultsTable = QueryExecutionStatisticsTable.CreateFromExecutionResult(executionResult);
                }
                else
                {
                    ResultsTable.AddRow(executionResult);
                }
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(T value, ref T field, [CallerMemberName] string propertyName = "")
        {
            field = value;
            NotifyPropertyChanged(propertyName);
        }
    }
}
