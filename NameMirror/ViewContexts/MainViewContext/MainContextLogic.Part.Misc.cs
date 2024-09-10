using NameMirror.Commands;
using NameMirror.Types;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands : Manage
    private readonly ActionCommand evaluateErrorsCommand;

    public ActionCommand EvaluateErrorsCommand => evaluateErrorsCommand;

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
            else if (!Handler.FileSystemAgent.FileExists(task.OriginalPath))
                missing++;
            else if (!task.Ready)
                notready++;
            else if (task.LastException != null)
                failed++;
            else
                uncat++;
        }

        LogAdd(
            $"Evaluation Completed:\n"
            + $"Completed={completed}\n"
            + $"NotReady={notready}\n"
            + $"Failed={failed}");
    }

    private readonly ActionCommand retaskCommand;
    public ActionCommand RetaskCommand => retaskCommand;

    private bool CanRetask(object? parameter) => Data.AtLeastOneTask;

    private void Retask(object? parameter)
    {
        foreach (RNTask task in Data.Selection)
        {
            task.SuccessStatus = null;
        }
    }
}