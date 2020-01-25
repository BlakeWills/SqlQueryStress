using SqlQueryStressEngine.Parameters;
using System;

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
            throw new NotImplementedException();
        }
    }
}
