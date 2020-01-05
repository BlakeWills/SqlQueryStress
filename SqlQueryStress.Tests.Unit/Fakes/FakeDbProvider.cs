namespace SqlQueryStressEngine.Tests.Unit.Fakes
{
    internal class FakeDbProvider : IDbProvider
    {
        public bool BeforeTestStartExecuted { get; set; }

        public void BeforeTestStart() => BeforeTestStartExecuted = true;

        public IQueryWorker GetQueryWorker() => new FakeQueryWorker();
    }
}
