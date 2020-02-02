using SqlQueryStressGUI.DbProviders;
using System;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterSettingsViewModelBuilder
    {
        private readonly IConnectionProvider _connectionProvider;

        public ParameterSettingsViewModelBuilder(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public ParameterSettingsViewModel Build(ParameterType parameterType, string name)
        {
            var viewModel = GetParameterSettingsViewModel(parameterType);
            viewModel.Name = name;

            return viewModel;
        }

        private ParameterSettingsViewModel GetParameterSettingsViewModel(ParameterType parameterType) => parameterType switch
        {
            ParameterType.RandomDateRange => new RandomDateRangeParameterSettings(),
            ParameterType.RandomNumber => new RandomNumberParameterSettings(),
            ParameterType.MSSQLQuery => new MssqlQueryParameterSettings(_connectionProvider),
            _ => throw new ArgumentException()
        };
    }
}
