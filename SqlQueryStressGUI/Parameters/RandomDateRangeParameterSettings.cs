using SqlQueryStressEngine.Parameters;
using System;

namespace SqlQueryStressGUI.Parameters
{
    public class RandomDateRangeParameterSettings : ParameterSettingsViewModel
    {
        public RandomDateRangeParameterSettings(string name) : base(name) { }
        
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

        public RandomDateRangeParameterSettings LinkedParameter { get; set; } = null;

        public override IParameterValueBuilder GetParameterValueBuilder()
        {
            var linkedParamBuilder = (RandomDateRangeParameterBuilder)LinkedParameter?.GetParameterValueBuilder();
            return new RandomDateRangeParameterBuilder(MinValue, MaxValue, MaxInterval, Name, linkedParamBuilder);
        }
    }
}
