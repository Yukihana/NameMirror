using CommunityToolkit.Mvvm.Input;
using NameMirror.Types;
using System.IO;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands

    public RelayCommand ClearCompletedCommand { get; }
    public RelayCommand ClearMissingCommand { get; }
    public RelayCommand ClearUnreadyCommand { get; }
    public RelayCommand ClearAllCommand { get; }

    // Handlers

    private bool CanClearCompleted() => Data.AtLeastOneTask;

    private void ClearCompleted()
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

    private bool CanClearMissing() => Data.AtLeastOneTask;

    private void ClearMissing()
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

    private bool CanClearUnready() => Data.AtLeastOneTask;

    private void ClearUnready()
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

    private bool CanClearAll() => Data.AtLeastOneTask;

    private void ClearAll(bool confirm = true)
    {
        if (!confirm ||
            _services.PromptAgent.Validate(
            _services.PromptAgent.Query("Clear all tasks?", "Confirmation", "Yes;Cancel", "information", "Yes"),
            "yes"))
        {
            Data.Tasks.Clear();
        }
    }
}