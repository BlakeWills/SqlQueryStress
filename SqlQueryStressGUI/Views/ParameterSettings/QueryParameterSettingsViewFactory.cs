using SqlQueryStressGUI.ViewModels;
using System;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views.ParameterSettings
{
    public class QueryParameterSettingsViewFactory
    {
        public UserControl GetQueryParameterSettingsView(QueryParameterSettings parameterSettings) => (parameterSettings) switch
        {
            RandomNumberQueryParameterSettings num => new RandomNumberView(num),
            RandomDateRangeQueryParameterSettings date => new RandomDateRangeView(date),
            _ => throw new ArgumentException()
        };
    }
}
