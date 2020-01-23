using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Parameters.Views
{
    /// <summary>
    /// Interaction logic for ParameterSettingsPanel.xaml
    /// </summary>
    public partial class ParameterSettingsPanel : UserControl
    {
        private readonly IViewFactory _viewFactory;

        public ParameterSettingsPanel()
        {
            InitializeComponent();
            _viewFactory = DiContainer.Instance.ServiceProvider.GetRequiredService<IViewFactory>();
        }

        static ParameterSettingsPanel()
        {
            ParameterSettingsProperty = DependencyProperty.Register(
                "ParameterSettings",
                typeof(ParameterSettingsViewModel),
                typeof(ParameterSettingsPanel),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSettingsChanged)));
        }

        public static readonly DependencyProperty ParameterSettingsProperty;

        public ParameterSettingsViewModel ParameterSettings
        {
            get
            {
                return (ParameterSettingsViewModel)GetValue(ParameterSettingsProperty);
            }
            set
            {
                SetValue(ParameterSettingsProperty, value);
            }
        }

        private static void OnSettingsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = (ParameterSettingsPanel)d;
            panel.SetSettingsPanelContent((ParameterSettingsViewModel)e.NewValue);
        }

        private void SetSettingsPanelContent(ParameterSettingsViewModel parameterSettingsViewModel)
        {
            // can't use the generic overload as we're using an abstract base class
            _panel.Content = _viewFactory.GetUserControl(parameterSettingsViewModel.GetType(), parameterSettingsViewModel);
        }
    }
}
