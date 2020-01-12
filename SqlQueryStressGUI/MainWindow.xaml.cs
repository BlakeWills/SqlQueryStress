using SqlQueryStressGUI.Views;
using System.Windows;

namespace SqlQueryStressGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(QueryStressTestPage queryStressTestPage)
        {
            InitializeComponent();
            Content = queryStressTestPage;
        }
    }
}
