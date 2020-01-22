using SqlQueryStressGUI.ViewModels;
using System;

namespace SqlQueryStressGUI.DesignTimeContexts
{
    public static class DesignContexts
    {
        private static IConnectionProvider _connectionProvider = new FakeConnectionProvider();

        private static DbProviderFactory _dbProviderFactory = new DbProviderFactory();

        private static DbCommandProvider _dbCommandProvider = new DbCommandProvider(_dbProviderFactory);

        private static QueryParameterViewModelBuilder _queryParameterViewModelBuilder => new QueryParameterViewModelBuilder(null, null);

        public static QueryStressTestViewModel QueryStressTestContext =>
            new QueryStressTestViewModel(_connectionProvider, _dbProviderFactory, _dbCommandProvider, _queryParameterViewModelBuilder, new ParameterWindowBuilder());

        public static ConnectionManagerViewModel ConnectionManagerContext =>
            new ConnectionManagerViewModel(_connectionProvider);

        public static AddEditConnectionViewModel AddEditConnectionContext =>
            new AddEditConnectionViewModel(_connectionProvider)
            {
                Name = "Test"
            };

        public static QueryParameterManagerViewModel ParameterManagerContext => new QueryParameterManagerViewModel(Array.Empty<QueryParameterViewModel>());
    }
}
