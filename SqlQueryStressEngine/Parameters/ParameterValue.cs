namespace SqlQueryStressEngine.Parameters
{
    public class ParameterValue
    {
        public ParameterValue(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public object Value { get; }
    }
}
