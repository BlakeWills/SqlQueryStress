using BetterTabs;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for TabbedQueryEditor.xaml
    /// </summary>
    public partial class TabbedQueryEditor : UserControl
    {
        public TabbedQueryEditor()
        {
            InitializeComponent();

            tabControl.Tabs.Clear();
            tabControl.AddedNewTab += OnNewTab;
            tabControl.SelectedTabChanged += OnSelectedTabChanged;
        }

        static TabbedQueryEditor()
        {
            SelectedQueryProperty = DependencyProperty.Register("SelectedQuery", typeof(string), typeof(TabbedQueryEditor));
        }

        public static readonly DependencyProperty SelectedQueryProperty;

        private void OnNewTab(object sender, AddTabEventArgs e)
        {
            var tab = e.NewTab;
            tab.TabTitle = $"Untitled {tabControl.Tabs.Count}";
            tab.TabContent = new QueryEditor();
        }

        private void OnQueryChanged(object sender, System.EventArgs e)
        {
            SelectedQuery = ((QueryEditor)sender).Query;
        }

        private void OnSelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            var newEditor = (QueryEditor)tabControl.SelectedContent;

            SelectedQuery = newEditor.Query;
            newEditor.QueryChanged += OnQueryChanged;

            if (e.OldSelection != null)
            {
                ((QueryEditor)e.OldSelection.TabContent).QueryChanged -= OnQueryChanged;
            }
        }

        public string SelectedQuery
        {
            get
            {
                return (string)GetValue(SelectedQueryProperty);
            }
            set
            {
                SetValue(SelectedQueryProperty, value);
            }
        }
    }
}
