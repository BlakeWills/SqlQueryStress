using System;

namespace SqlQueryStressEngine.Parameters
{
    public sealed class RandomDateRangeParameterBuilder : IParameterValueBuilder
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;
        private readonly DateTime _maxStartDate;
        private readonly TimeSpan _maxInterval;
        private readonly string _name;
        private readonly RandomDateRangeParameterBuilder _startDateBuilder;

        private ParameterValue _lastValue;

        public RandomDateRangeParameterBuilder(DateTime minDate, DateTime maxDate, TimeSpan maxInterval, string name)
            : this(minDate, maxDate, maxInterval, name, startDateBuilder: null) { }

        public RandomDateRangeParameterBuilder(DateTime minDate, DateTime maxDate, TimeSpan maxInterval, string name, RandomDateRangeParameterBuilder startDateBuilder)
        {
            _minDate = minDate;
            _maxDate = maxDate;
            _maxInterval = maxInterval;
            _name = name;
            _startDateBuilder = startDateBuilder;

            _maxStartDate = maxDate.Add(-maxInterval);
        }

        public ParameterValue GetNextValue()
        {
            DateTime dateTime;

            if (_startDateBuilder == null)
            {
                dateTime = GetStartDate();
            }
            else
            {
                dateTime = GetEndDate();
            }

            return _lastValue = new ParameterValue(_name, dateTime);
        }

        private DateTime GetStartDate()
        {
            var interval = (int)Math.Floor((_maxStartDate - _minDate).TotalSeconds);
            return _minDate.Add(TimeSpan.FromSeconds(RandomWrapper.Random.Next(interval)));
        }

        private DateTime GetEndDate()
        {
            var startDate = (DateTime)_startDateBuilder._lastValue.Value;
            var interval = (int)Math.Floor(_startDateBuilder._maxInterval.TotalSeconds);
            return startDate.Add(TimeSpan.FromSeconds(RandomWrapper.Random.Next(interval)));
        }

        public RandomDateRangeParameterBuilder GetEndDateBuilder(string name)
        {
            return new RandomDateRangeParameterBuilder(_minDate, _maxDate, _maxInterval, name, this);
        }
    }
}
