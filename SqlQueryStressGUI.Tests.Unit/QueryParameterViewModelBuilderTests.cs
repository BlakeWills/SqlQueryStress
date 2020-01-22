using FluentAssertions;
using NUnit.Framework;
using SqlQueryStressGUI.ViewModels;
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

        private List<QueryParameterViewModel> _expectedViewModels;
        private List<QueryParameterViewModel> _viewModels;
        private QueryParameterViewModelBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new QueryParameterViewModelBuilder(null, null);

            _expectedViewModels = new List<QueryParameterViewModel>
            {
                new QueryParameterViewModel(null, null) { Name = "@Start" },
                new QueryParameterViewModel(null, null) { Name = "@End" },
            };

            _viewModels = new List<QueryParameterViewModel>();
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
            var startParam = new QueryParameterViewModel(null, null) { Name = "@Start" };
            var endParam = new QueryParameterViewModel(null, null) { Name = "@End" };

            _viewModels = new List<QueryParameterViewModel> { startParam, endParam };

            _builder.UpdateQueryParameterViewModels(_query, ref _viewModels);

            // Use reference equality here to be sure we're getting the same objects back
            // We essentially want to make sure that nothing has changed
            Assert.AreEqual(startParam, _viewModels[0]);
            Assert.AreEqual(endParam, _viewModels[1]);
        }

        [Test]
        public void GivenUpdatedQueryWithParams_BuildsMissingViewModels()
        {
            var startParam = new QueryParameterViewModel(null, null) { Name = "@Start" };
            _viewModels = new List<QueryParameterViewModel> { startParam };

            _builder.UpdateQueryParameterViewModels(_query, ref _viewModels);

            Assert.AreEqual(startParam, _viewModels[0], "StartParam was already present and should not have been changed");
            _viewModels[1].Should().BeEquivalentTo(_expectedViewModels[1], "because it was missing from the input params");
        }

        [Test]
        public void GivenUpdatedQueryWithParams_RemovesParamsNoLongerRequired()
        {
            _query = "SELECT * FROM MyTable";

            var startParam = new QueryParameterViewModel(null, null) { Name = "@Start" };
            _viewModels = new List<QueryParameterViewModel> { startParam };

            _builder.UpdateQueryParameterViewModels(_query, ref _viewModels);

            CollectionAssert.IsEmpty(_viewModels);
        }
    }
}