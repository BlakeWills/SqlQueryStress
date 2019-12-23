namespace SqlQueryStressGUI.ViewModels
{
    public class ConnectionViewModel : ViewModel
    {
        private string _hostName;
        public string HostName
        {
            get => _hostName;
            set => SetProperty(value, ref _hostName);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(value, ref _username);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(value, ref _password);
        }
    }
}
