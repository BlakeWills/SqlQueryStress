namespace SqlQueryStressEngine
{
    public interface IQueryStressTest<out TDbProvider> where TDbProvider : IQueryWorker
    {
        void BeginInvoke();

        void Wait();
    }
}