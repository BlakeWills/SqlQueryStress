using SqlQueryStressEngine.Parameters;

namespace SqlQueryStressGUI.Parameters
{
    public abstract class ParameterSettingsViewModel : ViewModel
    {
        public string Name { get; set; }

        public abstract IParameterValueBuilder GetParameterValueBuilder();

        private ParameterSettingsViewModel _linkedParameter;
        public ParameterSettingsViewModel LinkedParameter
        {
            get => _linkedParameter;
            set
            {
                SetProperty(value, ref _linkedParameter);
                NotifyPropertyChanged(nameof(HasLinkedParameter));
            }
        }

        public bool HasLinkedParameter
        {
            get => LinkedParameter != null;
        }
    }
}
