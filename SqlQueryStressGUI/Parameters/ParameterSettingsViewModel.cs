using SqlQueryStressEngine.Parameters;

namespace SqlQueryStressGUI.Parameters
{
    public abstract class ParameterSettingsViewModel : ViewModel
    {
        public ParameterSettingsViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract IParameterValueBuilder GetParameterValueBuilder();
    }
}
