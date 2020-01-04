namespace SqlQueryStressEngine
{
    // Not sure about using an abstract class here as it isn't very discoverable.
    // Dev programming the client can't use intellisense to discover it's properties - they would have to know what type was coming through.
    // Could we use generics here instead?
    // Should we just use a dictionary instead of a type?
    public abstract class QueryExecutionStatistics
    {
        public abstract double ElapsedMilliseconds { get; }
    }
}
