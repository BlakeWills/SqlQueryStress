using SqlQueryStressEngine.Parameters;
using SqlQueryStressGUI.Views.ParameterSettings;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SqlQueryStressGUI.ViewModels
{
    public class QueryParameterViewModel : ViewModel
    {
        private readonly QueryParameterSettingsFactory _queryParameterSettingsFactory;
        private readonly QueryParameterSettingsViewFactory _queryParameterSettingsViewFactory;

        public QueryParameterViewModel(
            QueryParameterSettingsFactory queryParameterSettingsFactory,
            QueryParameterSettingsViewFactory queryParameterSettingsViewFactory)
        {
            _queryParameterSettingsFactory = queryParameterSettingsFactory;
            _queryParameterSettingsViewFactory = queryParameterSettingsViewFactory;

            ParameterTypes = Enum.GetNames(typeof(ParameterType));

            SaveAndCloseCommand = new CommandHandler((closeable) => Close((ICloseable)closeable));
        }

        public CommandHandler SaveAndCloseCommand { get; }

        public string Name { get; set; }

        private string _parameterType;
        public string Type
        {
            get => _parameterType;
            set
            {
                SetProperty(value, ref _parameterType);
                SetSettingsPanelContent();
            }
        }

        public IEnumerable<string> ParameterTypes { get; }

        public QueryParameterSettings Settings { get; set; }

        public QueryParameterViewModel LinkedParameter { get; set; }

        private UserControl _settingsPanelContent;
        public UserControl SettingsPanelContent
        {
            get => _settingsPanelContent;
            set => SetProperty(value, ref _settingsPanelContent);
        }

        private void SetSettingsPanelContent()
        {
            Settings = _queryParameterSettingsFactory.GetQueryParameterSettings((ParameterType)Enum.Parse(typeof(ParameterType), Type));
            SettingsPanelContent = _queryParameterSettingsViewFactory.GetQueryParameterSettingsView(Settings);
        }
    }

    public abstract class QueryParameterSettings : ViewModel
    {
        public abstract IParameterValueBuilder GetParameterValueBuilder(string name);
    }

    public class QueryParameterSettingsFactory
    {
        public QueryParameterSettings GetQueryParameterSettings(ParameterType parameterType) => parameterType switch
        {
            ParameterType.RandomDateRange => new RandomDateRangeQueryParameterSettings(),
            ParameterType.RandomNumber => new RandomNumberQueryParameterSettings(),
            _ => throw new ArgumentException()
        };
    }

    public class RandomNumberQueryParameterSettings : QueryParameterSettings
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public override IParameterValueBuilder GetParameterValueBuilder(string name)
        {
            return new RandomNumberParameterBuilder(MinValue, MaxValue, name);
        }
    }

    public class RandomDateRangeQueryParameterSettings : QueryParameterSettings
    {
        private DateTime _minValue;
        public DateTime MinValue
        {
            get => _minValue;
            set => SetProperty(value, ref _minValue);
        }

        private DateTime _maxValue;
        public DateTime MaxValue
        {
            get => _maxValue;
            set => SetProperty(value, ref _maxValue);
        }

        private TimeSpan _maxInterval;
        public TimeSpan MaxInterval
        {
            get => _maxInterval;
            set => SetProperty(value, ref _maxInterval);
        }

        public override IParameterValueBuilder GetParameterValueBuilder(string name)
        {
            return new RandomDateRangeParameterBuilder(MinValue, MaxValue, MaxInterval, name);
        }
    }

    public enum ParameterType
    {
        RandomNumber = 0,
        RandomDateRange = 1
    }
}
