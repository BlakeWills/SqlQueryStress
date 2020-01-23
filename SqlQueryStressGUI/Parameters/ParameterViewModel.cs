using System;
using System.Collections.Generic;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterViewModel : ViewModel
    {
        private readonly ParameterSettingsFactory _queryParameterSettingsFactory;

        public ParameterViewModel(ParameterSettingsFactory queryParameterSettingsFactory)
        {
            _queryParameterSettingsFactory = queryParameterSettingsFactory;

            ParameterTypes = Enum.GetNames(typeof(ParameterType));

            SaveAndCloseCommand = new CommandHandler((closeable) => Close((ICloseable)closeable));
        }

        public CommandHandler SaveAndCloseCommand { get; }

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

        public IEnumerable<string> ParameterTypes { get; }

        private ParameterSettingsViewModel _settings;
        public ParameterSettingsViewModel Settings
        {
            get => _settings;
            set => SetProperty(value, ref _settings);
        }

        private void UpdateParameterSettings()
        {
            var paramType = (ParameterType)Enum.Parse(typeof(ParameterType), Type);
            Settings = _queryParameterSettingsFactory.GetQueryParameterSettings(paramType, Name);
        }
    }
}
