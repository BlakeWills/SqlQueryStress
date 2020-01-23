using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl, INotifyPropertyChanged
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }

        static DateTimePicker()
        {
            SelectedDateTimeProperty = DependencyProperty.Register(
                "SelectedDateTime",
                typeof(DateTime),
                typeof(DateTimePicker),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(UpdateDateTime)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty SelectedDateTimeProperty;

        public DateTime SelectedDateTime
        {
            get
            {
                return (DateTime)GetValue(SelectedDateTimeProperty);
            }
            set
            {
                SetValue(SelectedDateTimeProperty, value);
            }
        }

        public DateTime Date
        {
            get => SelectedDateTime.Date;
            set
            {
                var time = SelectedDateTime.TimeOfDay;
                SelectedDateTime = value.Add(time);
            }
        }

        public TimeSpan Time
        {
            get => SelectedDateTime.TimeOfDay;
            set
            {
                var date = SelectedDateTime.Date;
                SelectedDateTime = date.Add(value);
            }
        }

        private static void UpdateDateTime(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
        {
            var datePicker = (DateTimePicker)d;
            datePicker.PropertyChanged?.Invoke(d, new PropertyChangedEventArgs(nameof(Date)));
            datePicker.PropertyChanged?.Invoke(d, new PropertyChangedEventArgs(nameof(Time)));
        }
    }
}
