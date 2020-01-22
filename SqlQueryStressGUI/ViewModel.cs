using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SqlQueryStressGUI
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(T value, ref T field, [CallerMemberName] string propertyName = "")
        {
            field = value;
            NotifyPropertyChanged(propertyName);
        }

        public virtual void Close(ICloseable closeable) => closeable.Close();
    }
}
