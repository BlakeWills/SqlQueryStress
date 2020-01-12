using SqlQueryStressGUI.ViewModels;

namespace SqlQueryStressGUI.DesignTimeContexts
{
    public static class DesignContexts
    {
        private static IConnectionProvider _connectionProvider = new FakeConnectionProvider();

        public static QueryStressTestViewModel QueryStressTestContext =>
            new QueryStressTestViewModel(_connectionProvider);

        public static ConnectionManagerViewModel ConnectionManagerContext =>
            new ConnectionManagerViewModel(_connectionProvider);

        public static AddEditConnectionViewModel AddEditConnectionContext =>
            new AddEditConnectionViewModel(_connectionProvider)
            {
                Name = "Test"
            };
    }
}
