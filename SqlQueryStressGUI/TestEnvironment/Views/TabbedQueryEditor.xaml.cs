using SqlQueryStressGUI.Controls;
using SqlQueryStressGUI.QueryStressTests;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SqlQueryStressGUI.TestEnvironment.Views
{
    /// <summary>
    /// Interaction logic for TabbedQueryEditor.xaml
    /// </summary>
    public partial class TabbedQueryEditor : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SelectedTestProperty;
        public static readonly DependencyProperty TestsProperty;
        public static readonly DependencyProperty NewQueryCommandProperty;

        private List<QueryTab> _queryTabs;

        public TabbedQueryEditor()
        {
            InitializeComponent();

            _queryTabs = new List<QueryTab>();
        }

        static TabbedQueryEditor()
        {
            TestsProperty = DependencyProperty.Register(
                nameof(Tests),
                typeof(ObservableCollection<QueryStressTestViewModel>),
                typeof(TabbedQueryEditor),
                new PropertyMetadata(new PropertyChangedCallback(OnTestsChangedCallback)));
            
            SelectedTestProperty = DependencyProperty.Register(
                nameof(SelectedTest),
                typeof(QueryStressTestViewModel),
                typeof(TabbedQueryEditor),
                new PropertyMetadata(new PropertyChangedCallback(OnSelectedTestChanged)));

            NewQueryCommandProperty = DependencyProperty.Register(nameof(NewQueryCommand), typeof(ICommand), typeof(TabbedQueryEditor));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand NewQueryCommand
        {
            get => (ICommand)GetValue(NewQueryCommandProperty);
            set => SetValue(NewQueryCommandProperty, value);
        }

        public ObservableCollection<QueryStressTestViewModel> Tests
        {
            get => (ObservableCollection<QueryStressTestViewModel>)GetValue(TestsProperty);
            set => SetValue(TestsProperty, value);
        }

        public QueryStressTestViewModel SelectedTest
        {
            get => (QueryStressTestViewModel)GetValue(SelectedTestProperty);
            set => SetValue(SelectedTestProperty, value);
        }

        private ObservableCollection<Tab> _tabs;
        public ObservableCollection<Tab> Tabs
        {
            get => _tabs;
            set
            {
                _tabs = value;
                NotifyPropertyChanged();
            }
        }

        private Tab _selectedTab;
        public Tab SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;

                TryGetQueryTab(value, out var queryTab);
                if (queryTab.Query != SelectedTest)
                {
                    SelectedTest = queryTab.Query;
                }

                NotifyPropertyChanged();
            }
        }

        private static void OnTestsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabbedEditor = (TabbedQueryEditor)d;

            var oldTests = (ObservableCollection<QueryStressTestViewModel>)e.OldValue;
            if (oldTests != null)
            {
                oldTests.CollectionChanged -= tabbedEditor.OnTestsCollectionChanged;
            }

            ((ObservableCollection<QueryStressTestViewModel>)e.NewValue).CollectionChanged += tabbedEditor.OnTestsCollectionChanged;

            tabbedEditor.OnTestsChanged();
        }

        private void OnTestsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnTestsChanged();
        }

        private void OnTestsChanged()
        {
            var queryTabs = new List<QueryTab>();

            int index = 0;
            foreach (var test in Tests)
            {
                index++;

                if(!TryGetQueryTab(test, out var queryTab))
                {
                    queryTab = QueryTab.BuildQueryTab(test, $"Untitled {index}");
                }
    
                queryTabs.Add(queryTab);
            }

            _queryTabs = queryTabs;
            Tabs = new ObservableCollection<Tab>(_queryTabs.Select(x => x.Tab));
        }

        private static void OnSelectedTestChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabbedEditor = (TabbedQueryEditor)d;
            var selectedTest = (QueryStressTestViewModel)e.NewValue;

            tabbedEditor.TryGetQueryTab(selectedTest, out var selectedQueryTab);
            tabbedEditor.SelectedTab = selectedQueryTab.Tab;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool TryGetQueryTab(QueryStressTestViewModel viewModel, out QueryTab queryTab)
        {
            queryTab = _queryTabs?.SingleOrDefault(x => x.Query == viewModel);
            return queryTab != null;
        }

        private bool TryGetQueryTab(Tab tab, out QueryTab queryTab)
        {
            queryTab = _queryTabs?.SingleOrDefault(x => x.Tab == tab);
            return queryTab != null;
        }

        private class QueryTab
        {
            public QueryStressTestViewModel Query { get; set; }

            public Tab Tab { get; set; }

            public static QueryTab BuildQueryTab(QueryStressTestViewModel viewModel, string tabName)
            {
                var queryEditor = new QueryEditor();
                queryEditor.QueryChanged += (sender, args) =>
                {
                    viewModel.Query = ((QueryEditor)sender).Query;
                };

                var tab = new Tab()
                {
                    HeaderText = tabName,
                    Content = queryEditor
                };

                return new QueryTab
                {
                    Query = viewModel,
                    Tab = tab
                };
            }
        }
    }
}
