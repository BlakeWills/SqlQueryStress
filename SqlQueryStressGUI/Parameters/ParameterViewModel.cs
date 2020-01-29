using System;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterViewModel : ViewModel
    {
        private readonly ParameterSettingsViewModelBuilder _parameterSettingsViewModelBuilder;

        public ParameterViewModel(
            string name,
            ParameterSettingsViewModelBuilder parameterSettingsViewModelBuilder)
        {
            _parameterSettingsViewModelBuilder = parameterSettingsViewModelBuilder;
            Name = name;

            Type = Enum.GetNames(typeof(ParameterType))[0];
        }

        public string Name { get; set; }

        private string _parameterType;
        public string Type
        {
            get => _parameterType;
            set
            {
                SetProperty(value, ref _parameterType);
                UpdateParameterSettings();
            }
        }

        private ParameterSettingsViewModel _settings;
        public ParameterSettingsViewModel Settings
        {
            get => _settings;
            set => SetProperty(value, ref _settings);
        }

        private void UpdateParameterSettings()
        {
            var paramType = (ParameterType)Enum.Parse(typeof(ParameterType), Type);
            Settings = _parameterSettingsViewModelBuilder.Build(paramType, Name);
        }
    }
}
