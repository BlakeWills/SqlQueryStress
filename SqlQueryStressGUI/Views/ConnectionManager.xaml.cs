using SqlQueryStressGUI.ViewModels;
using System.Windows;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for ConnectionManager.xaml
    /// </summary>
    public partial class ConnectionManager : Window, ICloseable
    {
        public ConnectionManager(ConnectionManagerViewModel connectionManagerViewModel)
        {
            InitializeComponent();
            DataContext = connectionManagerViewModel;
        }
    }
}
