using System;
using System.Collections.Generic;

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
            SettingsCommand = new CommandHandler((param) => OpenParameterSettings((ParameterViewModel)param));

            AvailableParameterTypes = Enum.GetNames(typeof(ParameterType));
        }

        public IEnumerable<string> AvailableParameterTypes { get; }

        public IEnumerable<ParameterViewModel> QueryParameters { get; }

        public CommandHandler SaveAndCloseCommand { get; }

        public CommandHandler SettingsCommand { get; }

        private void OpenParameterSettings(ParameterViewModel queryParameter) => _viewFactory.ShowDialog(queryParameter);
    }
}
