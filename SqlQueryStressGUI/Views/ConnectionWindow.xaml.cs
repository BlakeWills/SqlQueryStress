using SqlQueryStressGUI.ViewModels;
using System.Windows;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window, ICloseable
    {
        public ConnectionWindow(AddEditConnectionViewModel addEditConnectionViewModel)
        {
            InitializeComponent();
            DataContext = addEditConnectionViewModel;
        }
    }
}
