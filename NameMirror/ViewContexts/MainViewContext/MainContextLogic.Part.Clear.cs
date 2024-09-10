using NameMirror.Commands;
using NameMirror.Types;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands : Clear
    private readonly ActionCommand clearCompletedCommand;

    public ActionCommand ClearCompletedCommand => clearCompletedCommand;

    private bool CanClearCompleted(object? parameter) => Data.AtLeastOneTask;

    private void ClearCompleted(object? parameter = null)
    {
        // Adding confirmation to prevent accidents
        if (Handler.PromptAgent.Validate(
            Handler.PromptAgent.Query("Clear all completed tasks?", "Confirmation", "Yes;Cancel", "information", "Yes"),
            "yes"))
        {
            foreach (RNTask task in Data.Tasks)
            {
                if (task.SuccessStatus == true)
                {
                    Data.Tasks.Remove(task);
                }
            }
        }
    }

    private readonly ActionCommand clearMissingCommand;
    public ActionCommand ClearMissingCommand => clearMissingCommand;

    private bool CanClearMissing(object? parameter) => Data.AtLeastOneTask;

    private void ClearMissing(object? parameter = null)
    {
        if (Handler.PromptAgent.Validate(
            Handler.PromptAgent.Query("Clear all tasks with missing files?", "Confirmation", "Yes;Cancel", "information", "Yes"),
            "yes"))
        {
            foreach (RNTask task in Data.Tasks)
            {
                if (!Handler.FileSystemAgent.FileExists(task.OriginalPath))
                {
                    Data.Tasks.Remove(task);
                }
            }
        }
    }

    private readonly ActionCommand clearUnreadyCommand;
    public ActionCommand ClearUnreadyCommand => clearUnreadyCommand;

    private bool CanClearUnready(object? parameter) => Data.AtLeastOneTask;

    private void ClearUnready(object? parameter = null)
    {
        if (Handler.PromptAgent.Validate(
            Handler.PromptAgent.Query("Clear all tasks not ready for renaming?", "Confirmation", "Yes;Cancel", "information", "Yes"),
            "yes"))
        {
            foreach (RNTask task in Data.Tasks)
            {
                if (!task.Ready)
                {
                    Data.Tasks.Remove(task);
                }
            }
        }
    }

    private readonly ActionCommand clearAllCommand;
    public ActionCommand ClearAllCommand => clearAllCommand;

    private bool CanClearAll(object? parameter) => Data.AtLeastOneTask;

    private void ClearAll(object? parameter = null)
    {
        if (Handler.PromptAgent.Validate(
            Handler.PromptAgent.Query("Clear all tasks?", "Confirmation", "Yes;Cancel", "information", "Yes"),
            "yes"))
        {
            foreach (RNTask task in Data.Tasks)
            {
                Data.Tasks.Remove(task);
            }
        }
    }
}