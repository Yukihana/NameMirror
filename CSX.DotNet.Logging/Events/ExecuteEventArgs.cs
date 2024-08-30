using System;

namespace CSX.DotNet.Logging.Events
{
    public class ExecuteEventArgs : EventArgs
    {
        public object? Parameter = null;
        public ExecuteEventArgs() : base() { }
        public ExecuteEventArgs(object? parameter = null) : base() => Parameter = parameter;
    }
}