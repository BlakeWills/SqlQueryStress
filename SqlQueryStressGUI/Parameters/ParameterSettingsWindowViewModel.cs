using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterSettingsWindowViewModel : ViewModel
    {
        public ParameterSettingsWindowViewModel()
        {
            ParameterTypes = Enum.GetNames(typeof(ParameterType));

            SaveAndCloseCommand = new CommandHandler((closeable) => Close((ICloseable)closeable));
        }

        public CommandHandler SaveAndCloseCommand { get; }

        public IEnumerable<string> ParameterTypes { get; }

        public ParameterViewModel Parameter { get; set; }

        public string ParameterType
        {
            get => Parameter.Type;
            set
            {
                Parameter.Type = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(LinkedParameter));
            }
        }

        private IEnumerable<ParameterSettingsViewModel> _queryParameters;
        public IEnumerable<ParameterSettingsViewModel> QueryParameters
        {
            get => _queryParameters;
            set
            {
                _queryParameters = BuildQueryParameters(value);
                NotifyPropertyChanged(nameof(LinkedParameter));
            }
        }

        public ParameterSettingsViewModel LinkedParameter
        {
            get => Parameter.Settings.LinkedParameter ?? QueryParameters.FirstOrDefault(x => x is NullParameterSettingsViewModel);
            set
            {
                NotifyPropertyChanged();

                if(value is NullParameterSettingsViewModel)
                {
                    Parameter.Settings.LinkedParameter = null;
                }
                else
                {
                    Parameter.Settings.LinkedParameter = value;
                }
            }
        }

        private IEnumerable<ParameterSettingsViewModel> BuildQueryParameters(IEnumerable<ParameterSettingsViewModel> parameters)
        {
            var noLinkedParam = new NullParameterSettingsViewModel();
            return parameters.Where(x => x.Name != Parameter.Name).Concat(new[] { noLinkedParam });
        }

        public static ParameterSettingsWindowViewModel Build(
            IEnumerable<ParameterViewModel> parameterSettingsViewModels,
            ParameterViewModel parameterViewModel)
        {
            return new ParameterSettingsWindowViewModel()
            {
                QueryParameters = parameterSettingsViewModels.Select(x => x.Settings),
                Parameter = parameterViewModel
            };
        }
    }
}
