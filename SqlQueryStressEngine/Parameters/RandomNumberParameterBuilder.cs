namespace SqlQueryStressEngine.Parameters
{
    public class RandomNumberParameterBuilder : IParameterValueBuilder
    {
        private readonly int _min;
        private readonly int _max;
        private readonly string _name;

        public RandomNumberParameterBuilder(int min, int max, string name)
        {
            _min = min;
            _max = max;
            _name = name;
        }

        public ParameterValue GetNextValue() => new ParameterValue(_name, RandomWrapper.Random.Next(_min, _max));
    }
}
