using NameMirror.Commands;
using NameMirror.Types;
using System.IO;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands

    public ActionCommand ClearCompletedCommand { get; }
    public ActionCommand ClearMissingCommand { get; }
    public ActionCommand ClearUnreadyCommand { get; }
    public ActionCommand ClearAllCommand { get; }

    // Handlers

    private bool CanClearCompleted(object? parameter) => Data.AtLeastOneTask;

    private void ClearCompleted(object? parameter = null)
    {
        // Adding confirmation to prevent accidents
        if (_services.PromptAgent.Validate(
            _services.PromptAgent.Query("Clear all completed tasks?", "Confirmation", "Yes;Cancel", "information", "Yes"),
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

    private bool CanClearMissing(object? parameter) => Data.AtLeastOneTask;

    private void ClearMissing(object? parameter = null)
    {
        if (_services.PromptAgent.Validate(
            _services.PromptAgent.Query("Clear all tasks with missing files?", "Confirmation", "Yes;Cancel", "information", "Yes"),
            "yes"))
        {
            foreach (RNTask task in Data.Tasks)
            {
                if (!File.Exists(task.OriginalPath))
                {
                    Data.Tasks.Remove(task);
                }
            }
        }
    }

    private bool CanClearUnready(object? parameter) => Data.AtLeastOneTask;

    private void ClearUnready(object? parameter = null)
    {
        if (_services.PromptAgent.Validate(
            _services.PromptAgent.Query("Clear all tasks not ready for renaming?", "Confirmation", "Yes;Cancel", "information", "Yes"),
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

    private bool CanClearAll(object? parameter) => Data.AtLeastOneTask;

    private void ClearAll(object? parameter = null, bool confirm = true)
    {
        if (_services.PromptAgent.Validate(
            _services.PromptAgent.Query("Clear all tasks?", "Confirmation", "Yes;Cancel", "information", "Yes"),
            "yes"))
        {
            Data.Tasks.Clear();
        }
    }
}