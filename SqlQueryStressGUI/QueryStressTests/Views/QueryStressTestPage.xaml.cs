using System.Windows.Controls;

namespace SqlQueryStressGUI.QueryStressTests.Views
{
    /// <summary>
    /// Interaction logic for QueryStressTestPage.xaml
    /// </summary>
    public partial class QueryStressTestPage : Page
    {
        public QueryStressTestPage()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out int _);
        }
    }
}
