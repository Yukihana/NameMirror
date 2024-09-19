using NameMirror.Commands;
using NameMirror.Types;
using System.IO;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands

    public ActionCommand EvaluateErrorsCommand { get; }
    public ActionCommand RetaskCommand { get; }

    // Handlers

    private bool CanEvaluateErrors(object? parameter) => Data.AtLeastOneTask;

    private void EvaluateErrors(object? parameter)
    {
        int completed = 0;
        int notready = 0;
        int missing = 0;
        int failed = 0;
        int uncat = 0;

        foreach (RNTask task in Data.Tasks)
        {
            if (task.SuccessStatus == true)
                completed++;
            else if (!File.Exists(task.OriginalPath))
                missing++;
            else if (!task.Ready)
                notready++;
            else if (task.LastException != null)
                failed++;
            else
                uncat++;
        }

        Log(
            $"Evaluation Completed:\n"
            + $"Completed={completed}\n"
            + $"NotReady={notready}\n"
            + $"Failed={failed}");
    }

    private bool CanRetask(object? parameter) => Data.AtLeastOneTask;

    private void Retask(object? parameter)
    {
        foreach (RNTask task in Data.Selection)
        {
            task.SuccessStatus = null;
        }
    }
}