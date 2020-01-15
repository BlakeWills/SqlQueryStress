using FluentAssertions;
using NUnit.Framework;
using SqlQueryStressEngine.Parameters;
using System.Linq;

namespace SqlQueryStressEngine.Tests.Unit
{
    [TestFixture]
    public class ParameterProviderTests
    {
        [TestCase(5)]
        [TestCase(50)]
        public void GetParameterSets_ReturnsCorrectNumberOfSets(int numberOfSets)
        {
            var paramProvider = new ParameterProvider();
            var builders = new IParameterValueBuilder[] { new FakeParameterValueBuilder("ParamOne", 1) };

            var sets = paramProvider.GetParameterSets(builders, numberOfSets);

            Assert.AreEqual(numberOfSets, sets.Count());
        }

        [Test]
        public void GetParameterSets_ReturnsSetWithParameterFromEachBuilder()
        {
            var paramProvider = new ParameterProvider();
            var builders = new IParameterValueBuilder[]
            {
                new FakeParameterValueBuilder("ParamOne", 1),
                new FakeParameterValueBuilder("ParamTwo", 2)
            };

            var set = paramProvider.GetParameterSets(builders, numberOfSets: 1).Single();

            var expected = new ParameterSet();
            expected.Add(new ParameterValue("ParamOne", 1));
            expected.Add(new ParameterValue("ParamTwo", 2));

            set.Parameters.Should().BeEquivalentTo(expected.Parameters);
        }

        class FakeParameterValueBuilder : IParameterValueBuilder
        {
            private readonly string _paramName;
            private readonly object _paramValue;

            public FakeParameterValueBuilder(string paramName, object paramValue)
            {
                _paramName = paramName;
                _paramValue = paramValue;
            }

            public ParameterValue GetNextValue()
            {
                return new ParameterValue(_paramName, _paramValue);
            }
        }
    }
}
