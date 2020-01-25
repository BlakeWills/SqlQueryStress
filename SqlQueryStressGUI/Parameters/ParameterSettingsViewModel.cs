using SqlQueryStressEngine.Parameters;

namespace SqlQueryStressGUI.Parameters
{
    public abstract class ParameterSettingsViewModel : ViewModel
    {
        public string Name { get; protected set; }

        public abstract IParameterValueBuilder GetParameterValueBuilder();

        public ParameterSettingsViewModel LinkedParameter { get; set; }
    }
}
