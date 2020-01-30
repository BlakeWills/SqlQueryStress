using Microsoft.Extensions.DependencyInjection;
using System;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterSettingsViewModelBuilder
    {
        public ParameterSettingsViewModel Build(ParameterType parameterType, string name)
        {
            var paramSettingsType = GetParameterSettingsType(parameterType);
            var viewModel = (ParameterSettingsViewModel)DiContainer.Instance.ServiceProvider.GetRequiredService(paramSettingsType);
            viewModel.Name = name;

            return viewModel;
        }

        private Type GetParameterSettingsType(ParameterType parameterType) => parameterType switch
        {
            ParameterType.RandomDateRange => typeof(RandomDateRangeParameterSettings),
            ParameterType.RandomNumber => typeof(RandomNumberParameterSettings),
            ParameterType.MSSQLQuery => typeof(MssqlQueryParameterSettings),
            _ => throw new ArgumentException()
        };
    }
}
