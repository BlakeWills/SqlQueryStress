using SqlQueryStressEngine;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SqlQueryStressGUI.TestEnvironment.Views
{
    /// <summary>
    /// Interaction logic for QueryEditor.xaml
    /// </summary>
    public partial class QueryEditor : UserControl
    {
        public static readonly DependencyProperty QueryProperty;
        public static readonly DependencyProperty ResultsProperty;

        public QueryEditor()
        {
            InitializeComponent();

            sqlEditor.TextChanged += OnTextChanged;
        }

        static QueryEditor()
        {
            QueryProperty = DependencyProperty.Register("Query", typeof(string), typeof(QueryEditor));

            ResultsProperty = DependencyProperty.Register(
                "Results",
                typeof(ObservableCollection<QueryExecution>),
                typeof(QueryEditor));
        }

        public event EventHandler QueryChanged;

        public ObservableCollection<QueryExecution> Results
        {
            get => (ObservableCollection<QueryExecution>)GetValue(ResultsProperty);
            set => SetValue(ResultsProperty, value);
        }

        public Binding AvgExecutionTimeBinding
        {
            set => avgExecutionTimeLabel.SetBinding(ContentProperty, value);
        }

        public Binding ElapsedTimeBinding
        {
            set => elapsedTimeLabel.SetBinding(ContentProperty, value);
        }

        public Binding TestStateBinding
        {
            set => testStateLabel.SetBinding(ContentProperty, value);
        }

        public string Query
        {
            get => (string)GetValue(QueryProperty);
            set => SetValue(QueryProperty, value);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            Query = sqlEditor.Document.Text;
            QueryChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
