using SqlQueryStressGUI.ViewModels;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views.ParameterSettings
{
    /// <summary>
    /// Interaction logic for RandomDateRangeView.xaml
    /// </summary>
    public partial class RandomDateRangeView : UserControl
    {
        public RandomDateRangeView(RandomDateRangeQueryParameterSettings parameterSettings)
        {
            InitializeComponent();
            DataContext = parameterSettings;
        }
    }
}
