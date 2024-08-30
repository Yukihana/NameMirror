using System;

namespace CSX.DotNet6.Logging.Events
{
    public class CanExecuteEventArgs : EventArgs
    {
        public bool CanExecute = false;
        public object? Parameter = null;
        public CanExecuteEventArgs() : base() { }
        public CanExecuteEventArgs(object? parameter = null, bool canExecute = false)
        {
            CanExecute = canExecute;
            Parameter = parameter;
        }
    }
}