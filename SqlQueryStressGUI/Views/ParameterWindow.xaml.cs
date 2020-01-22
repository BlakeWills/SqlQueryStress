using SqlQueryStressGUI.ViewModels;
using System.Windows;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for ParameterWindow.xaml
    /// </summary>
    public partial class ParameterWindow : Window
    {
        public ParameterWindow(QueryParameterManagerViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
