using SqlQueryStressGUI.Parameters;
using SqlQueryStressGUI.ViewModels;
using System;

namespace SqlQueryStressGUI.DesignTimeContexts
{
    public static class DesignContexts
    {
        private static IConnectionProvider _connectionProvider = new FakeConnectionProvider();

        private static DbProviderFactory _dbProviderFactory = new DbProviderFactory();

        private static DbCommandProvider _dbCommandProvider = new DbCommandProvider(_dbProviderFactory);

        private static ParameterViewModelBuilder _queryParameterViewModelBuilder => new ParameterViewModelBuilder(null);

        public static QueryStressTestViewModel QueryStressTestContext =>
            new QueryStressTestViewModel(_connectionProvider, _dbProviderFactory, _dbCommandProvider, _queryParameterViewModelBuilder, viewFactory: null);

        public static ConnectionManagerViewModel ConnectionManagerContext =>
            new ConnectionManagerViewModel(_connectionProvider);

        public static AddEditConnectionViewModel AddEditConnectionContext =>
            new AddEditConnectionViewModel(_connectionProvider)
            {
                Name = "Test"
            };

        public static ParameterManagerViewModel ParameterManagerContext => new ParameterManagerViewModel(Array.Empty<ParameterViewModel>(), null);
    }
}
