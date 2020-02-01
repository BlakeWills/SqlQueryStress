using Microsoft.Extensions.DependencyInjection;
using SqlQueryStressEngine;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace SqlQueryStressGUI.TestEnvironment.Views
{
    /// <summary>
    /// Interaction logic for QueryExecutionResultsPanel.xaml
    /// </summary>
    public partial class QueryExecutionResultsPanel : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ResultsProperty;
        private readonly IViewFactory _viewFactory;

        public QueryExecutionResultsPanel()
        {
            InitializeComponent();
            _viewFactory = DiContainer.Instance.ServiceProvider.GetRequiredService<IViewFactory>();

            OpenExecutionDetailsCommand = new CommandHandler((executionRow) => OpenExecutionDetails((DataRowView)executionRow));
        }

        static QueryExecutionResultsPanel()
        {
            ResultsProperty = DependencyProperty.Register(
                "Results",
                typeof(ObservableCollection<QueryExecution>),
                typeof(QueryExecutionResultsPanel),
                new PropertyMetadata(new PropertyChangedCallback(ResultsChanged)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CommandHandler OpenExecutionDetailsCommand { get; }

        public ObservableCollection<QueryExecution> Results
        {
            get => (ObservableCollection<QueryExecution>)GetValue(ResultsProperty);
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

            var oldResults = (ObservableCollection<QueryExecution>)e.OldValue;
            if (oldResults != null)
            {
                oldResults.CollectionChanged -= resultsPanel.Results_CollectionChanged;
            }

            var results = (ObservableCollection<QueryExecution>)e.NewValue;
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
                var executionResult = (QueryExecution)result;

                if (ResultsTable.Rows.Count == 0)
                {
                    ResultsTable = QueryExecutionStatisticsTable.CreateFromExecutionResult(executionResult);

                    resultsDataGrid.Columns.Add(new DataGridTemplateColumn()
                    {
                        CellTemplate = GetExecutionDetailsDataTemplate()
                    });
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

        private void OpenExecutionDetails(DataRowView executionRow)
        {
            var queryExecution = ResultsTable.GetExecution(executionRow.Row);

            var executionDetailsViewModel = new QueryExecutionDetailsViewModel
            {
                QueryExecution = queryExecution
            };

            _viewFactory.ShowDialog(executionDetailsViewModel);
        }

        private static DataTemplate GetExecutionDetailsDataTemplate()
        {
            StringReader stringReader = new StringReader(
                @$"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""> 
                    <Button
                        Command = ""{{Binding ElementName={nameof(QueryExecutionResultsPanelUC)}, Path={nameof(OpenExecutionDetailsCommand)}}}""
                        CommandParameter = ""{{Binding ElementName={nameof(resultsDataGrid)}, Path=CurrentItem}}""
                        Content = ""Execution Details""
                        BorderThickness=""0""
                        Background = ""Transparent""
                        Foreground = ""Blue"" />
                  </DataTemplate>");

            var xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as DataTemplate;
        }
    }
}
