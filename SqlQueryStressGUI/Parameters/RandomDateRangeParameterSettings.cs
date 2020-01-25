using SqlQueryStressEngine.Parameters;
using System;

namespace SqlQueryStressGUI.Parameters
{
    public class RandomDateRangeParameterSettings : ParameterSettingsViewModel
    {
        private IParameterValueBuilder _parameterValueBuilder;

        public RandomDateRangeParameterSettings(string name)
        {
            Name = name;
        }
        
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

        public override IParameterValueBuilder GetParameterValueBuilder()
        {
            if (_parameterValueBuilder == null)
            {
                var linkedParamBuilder = (RandomDateRangeParameterBuilder)LinkedParameter?.GetParameterValueBuilder();
                _parameterValueBuilder = new RandomDateRangeParameterBuilder(MinValue, MaxValue, MaxInterval, Name, linkedParamBuilder);
            }

            return _parameterValueBuilder;
        }
    }
}
