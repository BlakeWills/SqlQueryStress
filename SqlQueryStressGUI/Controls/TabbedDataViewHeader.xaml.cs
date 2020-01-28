using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SqlQueryStressGUI.Controls
{
    /// <summary>
    /// Interaction logic for TabbedDataViewHeader.xaml
    /// </summary>
    public partial class TabbedDataViewHeader : UserControl, INotifyPropertyChanged
    {
        public static DependencyProperty HeaderProperty;
        public static DependencyProperty ClickedCommandProperty;

        public TabbedDataViewHeader()
        {
            InitializeComponent();
        }

        static TabbedDataViewHeader()
        {
            HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(TabbedDataViewHeader));
            ClickedCommandProperty = DependencyProperty.Register(nameof(ClickedCommand), typeof(ICommand), typeof(TabbedDataViewHeader));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public ICommand ClickedCommand
        {
            get => (ICommand)GetValue(ClickedCommandProperty);
            set => SetValue(ClickedCommandProperty, value);
        }

        private void OnHeaderClicked(object sender, MouseButtonEventArgs e)
        {
            ClickedCommand?.Execute(this);
            IsSelected = true;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
