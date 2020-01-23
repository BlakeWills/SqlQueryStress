using SqlQueryStressEngine.Parameters;

namespace SqlQueryStressGUI.Parameters
{
    public class RandomNumberParameterSettings : ParameterSettingsViewModel
    {
        public RandomNumberParameterSettings(string name) : base(name) { }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public override IParameterValueBuilder GetParameterValueBuilder()
        {
            return new RandomNumberParameterBuilder(MinValue, MaxValue, Name);
        }
    }
}
