using System;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for QueryEditor.xaml
    /// </summary>
    public partial class QueryEditor : UserControl
    {
        public QueryEditor()
        {
            InitializeComponent();

            sqlEditor.TextChanged += OnTextChanged;
        }

        static QueryEditor()
        {
            QueryProperty = DependencyProperty.Register("Query", typeof(string), typeof(QueryEditor));
        }

        public event EventHandler QueryChanged;

        public static readonly DependencyProperty QueryProperty;

        public string Query
        {
            get => (string)GetValue(QueryProperty);
            set => SetValue(QueryProperty, value);
        }

        private void OnTextChanged(object sender, System.EventArgs e)
        {
            Query = sqlEditor.Document.Text;
            QueryChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
