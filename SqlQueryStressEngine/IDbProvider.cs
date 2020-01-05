namespace SqlQueryStressEngine
{
    public interface IDbProvider
    {
        void BeforeTestStart();

        IQueryWorker GetQueryWorker();
    }
}
