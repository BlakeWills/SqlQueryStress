using FluentAssertions;
using NUnit.Framework;
using SqlQueryStressGUI.Parameters;
using System.Collections.Generic;

namespace SqlQueryStressGUI.Tests.Unit
{
    public class QueryParameterViewModelBuilderTests
    {
        private string _query = @"
			SELECT
				ProductID,
				DATEFROMPARTS(YEAR(soh.OrderDate), MONTH(soh.OrderDate), 01) [Date],
				SUM(UnitPrice * OrderQty) [TotalSales]
			FROM
				[Sales].[SalesOrderDetail] sod
				JOIN [Sales].[SalesOrderHeader] soh ON soh.SalesOrderID = sod.SalesOrderID
            WHERE
				soh.OrderDate BETWEEN @Start AND @End
			GROUP BY
				ProductID,
				DATEFROMPARTS(YEAR(soh.OrderDate), MONTH(soh.OrderDate), 01)
			ORDER BY
				[Date] DESC,
				TotalSales DESC";

        private List<ParameterViewModel> _expectedViewModels;
        private List<ParameterViewModel> _viewModels;
        private ParameterViewModelBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new ParameterViewModelBuilder(null);

            _expectedViewModels = new List<ParameterViewModel>
            {
                new ParameterViewModel(null) { Name = "@Start" },
                new ParameterViewModel(null) { Name = "@End" },
            };

            _viewModels = new List<ParameterViewModel>();
        }

        [Test]
        public void GivenQueryWithParams_NoExistingViewModels_BuildsNewViewModels()
        {
            _builder.UpdateQueryParameterViewModels(_query, ref _viewModels);

            _viewModels.Should().BeEquivalentTo(_expectedViewModels);
        }

        [Test]
        public void GivenQueryWithParams_MatchingExistingViewModels_MakesNoChanges()
        {
            var startParam = new ParameterViewModel(null) { Name = "@Start" };
            var endParam = new ParameterViewModel(null) { Name = "@End" };

            _viewModels = new List<ParameterViewModel> { startParam, endParam };

            _builder.UpdateQueryParameterViewModels(_query, ref _viewModels);

            // Use reference equality here to be sure we're getting the same objects back
            // We essentially want to make sure that nothing has changed
            Assert.AreEqual(startParam, _viewModels[0]);
            Assert.AreEqual(endParam, _viewModels[1]);
        }

        [Test]
        public void GivenUpdatedQueryWithParams_BuildsMissingViewModels()
        {
            var startParam = new ParameterViewModel(null) { Name = "@Start" };
            _viewModels = new List<ParameterViewModel> { startParam };

            _builder.UpdateQueryParameterViewModels(_query, ref _viewModels);

            Assert.AreEqual(startParam, _viewModels[0], "StartParam was already present and should not have been changed");
            _viewModels[1].Should().BeEquivalentTo(_expectedViewModels[1], "because it was missing from the input params");
        }

        [Test]
        public void GivenUpdatedQueryWithParams_RemovesParamsNoLongerRequired()
        {
            _query = "SELECT * FROM MyTable";

            var startParam = new ParameterViewModel(null) { Name = "@Start" };
            _viewModels = new List<ParameterViewModel> { startParam };

            _builder.UpdateQueryParameterViewModels(_query, ref _viewModels);

            CollectionAssert.IsEmpty(_viewModels);
        }
    }
}