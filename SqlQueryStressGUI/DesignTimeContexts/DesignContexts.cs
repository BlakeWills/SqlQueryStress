using SqlQueryStressGUI.ViewModels;

namespace SqlQueryStressGUI.DesignTimeContexts
{
    public static class DesignContexts
    {
        private static IConnectionProvider _connectionProvider = new FakeConnectionProvider();

        private static DbProviderFactory _dbProviderFactory = new DbProviderFactory();

        private static DbCommandProvider _dbCommandProvider = new DbCommandProvider(_dbProviderFactory);

        public static QueryStressTestViewModel QueryStressTestContext =>
            new QueryStressTestViewModel(_connectionProvider, _dbProviderFactory, _dbCommandProvider);

        public static ConnectionManagerViewModel ConnectionManagerContext =>
            new ConnectionManagerViewModel(_connectionProvider);

        public static AddEditConnectionViewModel AddEditConnectionContext =>
            new AddEditConnectionViewModel(_connectionProvider)
            {
                Name = "Test"
            };
    }
}
