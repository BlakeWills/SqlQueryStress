using System;
using System.Collections.Generic;

namespace SqlQueryStressEngine.Tests.Unit.Fakes
{
    internal class FakeDbProvider : IDbProvider
    {
        public bool BeforeTestStartExecuted { get; set; }

        public void BeforeTestStart() => BeforeTestStartExecuted = true;

        public IEnumerable<IDbCommand> GetDbCommands() => Array.Empty<IDbCommand>();

        public IQueryWorker GetQueryWorker() => new FakeQueryWorker();
    }
}
