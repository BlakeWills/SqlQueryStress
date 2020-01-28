using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Controls
{
    /// <summary>
    /// Interaction logic for TabbedDataView.xaml
    /// </summary>
    public partial class TabbedDataView : UserControl
    {
        public static DependencyProperty ItemsSourceProperty;
        public static DependencyProperty SelectedItemProperty;

        public TabbedDataView()
        {
            InitializeComponent();
        }

        static TabbedDataView()
        {
            ItemsSourceProperty = DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(ObservableCollection<Tab>),
                typeof(TabbedDataView),
                new PropertyMetadata(new PropertyChangedCallback(OnItemsSourceChanged)));

            SelectedItemProperty = DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(Tab),
                typeof(TabbedDataView),
                new FrameworkPropertyMetadata(
                    default(Tab),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnSelectedItemChanged)));
        }

        public Tab SelectedItem
        {
            get => (Tab)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public ObservableCollection<Tab> ItemsSource
        {
            get => (ObservableCollection<Tab>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabbedDataView = (TabbedDataView)d;
            var selectedTab = (Tab)e.NewValue;
            var oldSelectedTab = (Tab)e.OldValue;

            if (oldSelectedTab != null)
            {
                oldSelectedTab.Header.IsSelected = false;
            }

            tabbedDataView._contentPanel.Content = selectedTab.Content;
            selectedTab.Header.IsSelected = true;
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabbedDataView = (TabbedDataView)d;
            var oldCollection = (ObservableCollection<Tab>)e.OldValue;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= tabbedDataView.OnItemsSourceCollectionChanged;
            }

            ((ObservableCollection<Tab>)e.NewValue).CollectionChanged += tabbedDataView.OnItemsSourceCollectionChanged;

            tabbedDataView.UpdateTabHeaders();
        }

        private void OnItemsSourceCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateTabHeaders();
        }

        private void UpdateTabHeaders()
        {
            _tabHeaderBar.Children.Clear();

            var headerClickedCommand = new CommandHandler((tabHeader) =>
            {
                var tab = GetTabForHeader((TabbedDataViewHeader)tabHeader);
                SelectedItem = tab;
            });

            foreach (var tab in ItemsSource)
            {
                var tabHeader = tab.BuildHeader(headerClickedCommand);
                _tabHeaderBar.Children.Add(tabHeader);
            }

            _tabHeaderBar.UpdateLayout();
        }

        private Tab GetTabForHeader(TabbedDataViewHeader header)
        {
            return ItemsSource.First(x => x.Header == header);
        }
    }

    public class Tab
    {
        public string HeaderText { get; set; }

        public Control Content { get; set; }

        public TabbedDataViewHeader Header { get; private set; }

        public TabbedDataViewHeader BuildHeader(CommandHandler clickedCommandHandler)
        {
            var header = new TabbedDataViewHeader()
            {
                Header = HeaderText,
                ClickedCommand = clickedCommandHandler
            };

            return Header = header;
        }
    }
}
