using System;
using System.Windows;
using System.Windows.Controls;

namespace SqlQueryStressGUI.Views
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }

        static DateTimePicker()
        {
            SelectedDateTimeProperty = DependencyProperty.Register("SelectedDateTime", typeof(DateTime), typeof(DateTimePicker));
        }

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

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                SetSelectedDateTime();
            }
        }

        private TimeSpan _time;
        public TimeSpan Time
        {
            get => _time;
            set
            {
                _time = value;
                SetSelectedDateTime();
            }
        }

        private void SetSelectedDateTime() => SelectedDateTime = Date.Add(Time);
    }
}
