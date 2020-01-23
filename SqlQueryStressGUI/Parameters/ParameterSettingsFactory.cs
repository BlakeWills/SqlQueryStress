using System;

namespace SqlQueryStressGUI.Parameters
{
    public class ParameterSettingsFactory
    {
        public ParameterSettingsViewModel GetQueryParameterSettings(ParameterType parameterType, string name) => parameterType switch
        {
            ParameterType.RandomDateRange => new RandomDateRangeParameterSettings(name),
            ParameterType.RandomNumber => new RandomNumberParameterSettings(name),
            _ => throw new ArgumentException()
        };
    }
}
