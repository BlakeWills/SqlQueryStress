using Microsoft.Extensions.DependencyInjection;
using SqlQueryStressGUI.Views.ParameterSettings;
using System;
using System.Collections.Generic;

namespace SqlQueryStressGUI.ViewModels
{
    public class QueryParameterManagerViewModel : ViewModel
    {
        public QueryParameterManagerViewModel(IEnumerable<QueryParameterViewModel> queryParameters)
        {
            QueryParameters = queryParameters;
            SettingsCommand = new CommandHandler((param) => OpenParameterSettings((QueryParameterViewModel)param));

            AvailableParameterTypes = Enum.GetNames(typeof(ParameterType));
        }

        public IEnumerable<string> AvailableParameterTypes { get; }

        public IEnumerable<QueryParameterViewModel> QueryParameters { get; }

        public CommandHandler SaveAndCloseCommand { get; }

        public CommandHandler SettingsCommand { get; }

        private void OpenParameterSettings(QueryParameterViewModel queryParameter)
        {
            var paramSettingsWindow = DiContainer.Instance.ServiceProvider.GetRequiredService<ParameterSettingsWindow>();
            paramSettingsWindow.DataContext = queryParameter;
            paramSettingsWindow.ShowDialog();
        }
    }
}
