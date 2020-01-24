using SqlQueryStressGUI.QueryStressTests;
using System.Windows;

namespace SqlQueryStressGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IViewFactory viewFactory)
        {
            InitializeComponent();
            Content = viewFactory.GetPage<QueryStressTestViewModel>();
        }
    }
}
