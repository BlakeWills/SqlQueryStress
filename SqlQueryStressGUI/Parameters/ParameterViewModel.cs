using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterViewModel : ViewModel
    {
        private readonly QueryParameterContext _queryParameterContext;

        public ParameterViewModel(
            string name,
            QueryParameterContext queryParameterContext)
        {
            _queryParameterContext = queryParameterContext;

            Name = name;

            ParameterTypes = Enum.GetNames(typeof(ParameterType));
            Type = ParameterTypes.First();

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

        public IEnumerable<ParameterSettingsViewModel> QueryParameters
        {
            get => GetPotentialLinkedParameters();
        }

        public ParameterSettingsViewModel LinkedParameter
        {
            get => Settings.LinkedParameter;
            set
            {
                if(!(value is NullParameterSettingsViewModel))
                {
                    Settings.LinkedParameter = value;
                }
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
            Settings = _queryParameterContext.UpdateQueryParameterSettings(paramType, Name);

            NotifyPropertyChanged(nameof(QueryParameters));
        }

        private IEnumerable<ParameterSettingsViewModel> GetPotentialLinkedParameters()
        {
            var noLinkedParam = new NullParameterSettingsViewModel();

            return _queryParameterContext.GetParameterSettings().Where(x => x.Name != Name).Concat(new[] { noLinkedParam });
        }
    }
}
