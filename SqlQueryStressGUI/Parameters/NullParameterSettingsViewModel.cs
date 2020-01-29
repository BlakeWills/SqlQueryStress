using SqlQueryStressEngine.Parameters;

namespace SqlQueryStressGUI.Parameters
{
    public class NullParameterSettingsViewModel : ParameterSettingsViewModel
    {
        public NullParameterSettingsViewModel()
        {
            Name = "N/A";
        }

        public override IParameterValueBuilder GetParameterValueBuilder()
        {
            throw new System.NotImplementedException();
        }
    }
}
