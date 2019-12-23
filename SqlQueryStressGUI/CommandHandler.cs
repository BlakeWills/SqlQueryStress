using System;
using System.Windows;
using System.Windows.Input;

namespace SqlQueryStressGUI
{
    public class CommandHandler : ICommand
    {
        private Action<object> _action;

        public CommandHandler(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return Enabled;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;

                    if (CanExecuteChanged != null)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() => CanExecuteChanged(this, null)));
                    }
                }
            }
        }
    }
}
