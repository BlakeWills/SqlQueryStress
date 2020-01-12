using SqlQueryStressGUI.ViewModels;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for QueryStressTestPage.xaml
    /// </summary>
    public partial class QueryStressTestPage : Page
    {
        public QueryStressTestPage(QueryStressTestViewModel queryStressTestViewModel)
        {
            DataContext = queryStressTestViewModel;
            InitializeComponent();
        }
    }
}
