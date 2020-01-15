using NUnit.Framework;
using SqlQueryStressEngine.Parameters;
using System;

namespace SqlQueryStressEngine.Tests.Unit
{
    [TestFixture]
    public class RandomDateRangeParameterBuilderTests
    {
        private DateTime _minDate;
        private DateTime _maxDate;
        private TimeSpan _maxInterval;
        private string _startDateName;
        private string _endDateName;

        private RandomDateRangeParameterBuilder _startDateBuilder;
        private RandomDateRangeParameterBuilder _endDateBuilder;

        [SetUp]
        public void TestSetup()
        {
            _minDate = new DateTime(2020, 01, 01, 00, 00, 00);
            _maxDate = new DateTime(2020, 01, 02, 23, 59, 59);
            _maxInterval = new TimeSpan(hours: 6, minutes: 0, seconds: 0);
            _startDateName = "startDate";
            _endDateName = "endDate";

            _startDateBuilder = new RandomDateRangeParameterBuilder(_minDate, _maxDate, _maxInterval, _startDateName);
            _endDateBuilder = _startDateBuilder.GetEndDateBuilder(_endDateName);
        }

        [Test]
        public void GetNextValue_WhenNoLinkedBuilder_GivenInterval_ReturnsDateWithinRange()
        {
            var startDate = (DateTime)_startDateBuilder.GetNextValue().Value;

            var maxStartDate = _maxDate.Add(-_maxInterval);

            Assert.IsTrue(startDate >= _minDate, "StartDate should be greater than or equal to minDate");
            Assert.IsTrue(startDate <= maxStartDate, "StartDate should be less than EndDate - maxInterval");
        }

        [Test]
        public void GetNextValue_GivenLinkedBuilder_ReturnsValidEndDate()
        {
            var startDate = (DateTime)_startDateBuilder.GetNextValue().Value;
            var endDate = (DateTime)_endDateBuilder.GetNextValue().Value;

            Assert.IsTrue(endDate > startDate, "EndDate should be greater than StartDate");
            Assert.IsTrue(endDate <= _maxDate, "EndDate should be less than max date");

            var dateDiff = (endDate - startDate);
            Assert.IsTrue(dateDiff <= _maxInterval, "Interval between start and end date should be less than max interval");
        }
    }
}
