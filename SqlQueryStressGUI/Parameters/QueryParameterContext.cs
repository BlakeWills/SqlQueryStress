using System;
using System.Collections.Generic;

namespace SqlQueryStressGUI.Parameters
{
    public class QueryParameterContext
    {
        private Dictionary<string, ParameterSettingsViewModel> _parameterSettings;

        public QueryParameterContext()
        {
            _parameterSettings = new Dictionary<string, ParameterSettingsViewModel>();
        }
        
        public void ClearParameters() => _parameterSettings.Clear();

        public IEnumerable<ParameterSettingsViewModel> GetParameterSettings() => _parameterSettings.Values;

        public ParameterSettingsViewModel UpdateQueryParameterSettings(ParameterType parameterType, string name)
        {
            var paramSettings = BuildQueryParameterSettings(parameterType, name);

            if (_parameterSettings.ContainsKey(name))
            {
                _parameterSettings[name] = paramSettings;
            }
            else
            {
                _parameterSettings.Add(name, paramSettings);
            }

            return paramSettings;
        }

        private ParameterSettingsViewModel BuildQueryParameterSettings(ParameterType parameterType, string name) => parameterType switch
        {
            ParameterType.RandomDateRange => new RandomDateRangeParameterSettings(name),
            ParameterType.RandomNumber => new RandomNumberParameterSettings(name),
            _ => throw new ArgumentException()
        };
    }
}
