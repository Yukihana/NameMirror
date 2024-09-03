using System;
using System.Diagnostics;
using System.Windows.Input;

namespace NameMirror.Commands;

public class ActionCommand : ICommand
{
    // Setup
    public ActionCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        ExecuteAction = execute;
        Avail = canExecute;
    }

    // Notify
    public event EventHandler? CanExecuteChanged;

    public void UpdateCanExecute() => CanExecuteChanged?.Invoke(this, new());

    // Avail
    private readonly Func<object?, bool>? Avail;

    [DebuggerStepThrough]
    public bool CanExecute(object? parameter) => Avail == null || Avail(parameter);

    // Actual
    private readonly Action<object?> ExecuteAction;

    public void Execute(object? parameter) => ExecuteAction(parameter);
}