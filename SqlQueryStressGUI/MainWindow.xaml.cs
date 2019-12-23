using SqlQueryStressGUI.Views;
using System.Windows;

namespace SqlQueryStressGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Content = new QueryStressTestPage();
        }
    }
}
