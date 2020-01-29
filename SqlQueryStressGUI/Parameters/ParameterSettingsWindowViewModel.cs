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

        private IEnumerable<ParameterSettingsViewModel> _queryParameters;
        public IEnumerable<ParameterSettingsViewModel> QueryParameters
        {
            get => _queryParameters;
            set => _queryParameters = BuildQueryParameters(value);
        }

        public ParameterViewModel Parameter { get; set; }

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
