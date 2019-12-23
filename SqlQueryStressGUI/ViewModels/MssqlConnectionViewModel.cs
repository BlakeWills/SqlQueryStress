namespace SqlQueryStressGUI.ViewModels
{
    public class MssqlConnectionViewModel : ConnectionViewModel
    {
        private string _databaseName;
        public string DatabaseName
        {
            get => _databaseName;
            set => SetProperty(value, ref _databaseName);
        }
    }
}
