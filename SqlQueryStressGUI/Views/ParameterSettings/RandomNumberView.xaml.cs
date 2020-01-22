using SqlQueryStressGUI.ViewModels;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views.ParameterSettings
{
    /// <summary>
    /// Interaction logic for RandomNumberView.xaml
    /// </summary>
    public partial class RandomNumberView : UserControl
    {
        public RandomNumberView(RandomNumberQueryParameterSettings parameterSettings)
        {
            InitializeComponent();
            DataContext = parameterSettings;
        }
    }
}
