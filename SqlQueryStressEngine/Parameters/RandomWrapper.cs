using System;

namespace SqlQueryStressEngine.Parameters
{
    public static class RandomWrapper
    {
        private static Lazy<Random> _random { get; } = new Lazy<Random>();

        public static Random Random => _random.Value;
    }
}
