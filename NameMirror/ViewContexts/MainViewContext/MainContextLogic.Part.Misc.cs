using CommunityToolkit.Mvvm.Input;
using NameMirror.Types;
using System.IO;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands

    public RelayCommand EvaluateErrorsCommand { get; }
    public RelayCommand RetaskCommand { get; }

    // Handlers

    private bool CanEvaluateErrors() => Data.AtLeastOneTask;

    private void EvaluateErrors()
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

    private bool CanRetask() => Data.AtLeastOneTask;

    private void Retask()
    {
        foreach (RNTask task in Data.Selection)
        {
            task.SuccessStatus = null;
        }
    }
}