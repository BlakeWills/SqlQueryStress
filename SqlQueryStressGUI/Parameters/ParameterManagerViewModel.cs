using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterManagerViewModel : ViewModel
    {
        private readonly IViewFactory _viewFactory;

        public ParameterManagerViewModel(
            IEnumerable<ParameterViewModel> queryParameters,
            IViewFactory viewFactory)
        {
            QueryParameters = queryParameters;
            _viewFactory = viewFactory;

            AvailableParameterTypes = Enum.GetNames(typeof(ParameterType));

            SettingsCommand = new CommandHandler((param) => OpenParameterSettings((ParameterViewModel)param));
            SaveAndCloseCommand = new CommandHandler((window) => Close((ICloseable)window));
        }

        public IEnumerable<string> AvailableParameterTypes { get; }

        public IEnumerable<ParameterViewModel> QueryParameters { get; }

        public CommandHandler SaveAndCloseCommand { get; }

        public CommandHandler SettingsCommand { get; }

        private void OpenParameterSettings(ParameterViewModel queryParameter)
        {
            var viewModel = ParameterSettingsWindowViewModel.Build(QueryParameters, queryParameter);

            _viewFactory.ShowDialog(viewModel);
        }
    }
}
