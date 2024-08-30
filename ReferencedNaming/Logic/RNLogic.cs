using ReferencedNaming.Agents;
using ReferencedNaming.Commands;
using ReferencedNaming.Models;
using ReferencedNaming.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ReferencedNaming.Logic;

public partial class RNLogic : INotifyPropertyChanged
{
    // Components
    private RNData data = new();

    public RNData Data
    {
        get => data;
        set
        {
            if (data != value)
            {
                data = value;
                PropertyChanged?.Invoke(this, new(nameof(Data)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    // Handler
    private readonly IHandler _handler;

    public IHandler Handler => _handler;

    public Action<string, string, object?>? LogAction = null;

    private void LogAdd(string message, string level = "information", object? sender = null)
        => LogAction?.Invoke(message, level, sender ?? this);

    // Validations
    private void Data_TasksChanged(object? sender, EventArgs e)
    {
        // Rename
        RenameAllCommand.UpdateCanExecute();

        // Evaluate
        EvaluateErrorsCommand.UpdateCanExecute();

        // Purge
        ClearMissingCommand.UpdateCanExecute();
        ClearUnreadyCommand.UpdateCanExecute();
        ClearAllCommand.UpdateCanExecute();
        ClearCompletedCommand.UpdateCanExecute();
    }

    private void Data_SelectionChanged(object? sender, EventArgs e)
    {
        // Rename
        RenameSelectedCommand.UpdateCanExecute();

        // Tasks
        TaskRemoveCommand.UpdateCanExecute();

        // References
        ReferenceRemoveCommand.UpdateCanExecute();

        // Sort
        TaskMoveUpCommand.UpdateCanExecute();
        TaskMoveDownCommand.UpdateCanExecute();
        TaskMoveToTopCommand.UpdateCanExecute();
        TaskMoveToBottomCommand.UpdateCanExecute();

        ReferenceMoveUpCommand.UpdateCanExecute();
        ReferenceMoveDownCommand.UpdateCanExecute();
        ReferenceMoveToTopCommand.UpdateCanExecute();
        ReferenceMoveToBottomCommand.UpdateCanExecute();
    }

    // Commands : Task
    private readonly ActionCommand taskRemoveCommand;

    public ActionCommand TaskRemoveCommand => taskRemoveCommand;

    private bool CanRemoveTask(object? parameter) => Data.AtLeastOneSelected;

    private void RemoveTask(object? parameter)
    {
        // Adding confirmation to prevent accidents
        if (!Handler.PromptAgent.Validate(
            Handler.PromptAgent.Query("Delete selected task(s)?", "Confirmation", "Yes;No", "information", "yes", "no"),
            "yes"))
            return;

        int successes = 0;

        // Create localized shallow copy to prevent cyclic stack overflow
        List<RNTask> ToRemove = new(Data.Selection);

        // Remove from master collection
        foreach (var task in ToRemove)
        {
            if (Data.Tasks.Contains(task))
            {
                Data.Tasks.Remove(task);
                successes++;
            }
        }

        LogAdd($"Removed {successes} tasks.");
    }

    private readonly ActionCommand taskMoveUpCommand;
    public ActionCommand TaskMoveUpCommand => taskMoveUpCommand;

    private bool CanMoveTaskUp(object? parameter)
        => Data.AtLeastOneSelected && !Data.SelectionHasMinimum;

    private void MoveTaskUp(object? parameter)
    {
        // Snapshot
        var selected = GetSelection().ToArray();
        int index;
        int successes = 0;

        // foreach: if index isn't minimum and previous item isn't also among selected, then move current up
        for (int i = 0; i < selected.Length; i++)
        {
            index = Data.Tasks.IndexOf(selected[i]);
            if (index != 0 && !selected.Contains(Data.Tasks[index - 1]))
            {
                (Data.Tasks[index], Data.Tasks[index - 1]) = (Data.Tasks[index - 1], Data.Tasks[index]);
                successes++;
            }
        }

        // Finish
        ConcludeReordering(successes, selected);
    }

    private readonly ActionCommand taskMoveDownCommand;
    public ActionCommand TaskMoveDownCommand => taskMoveDownCommand;

    private bool CanMoveTaskDown(object? parameter)
        => Data.AtLeastOneSelected && !Data.SelectionHasMaximum;

    private void MoveTaskDown(object? parameter)
    {
        // Snapshot
        RNTask[] selected = [.. GetSelection()];
        int index;
        int successes = 0;

        // foreach: if index isn't maximum and next item isn't also among selected, then move current down
        for (int i = selected.Length - 1; i >= 0; i--)
        {
            index = Data.Tasks.IndexOf(selected[i]);
            if (index != Data.Tasks.Count - 1 && !selected.Contains(Data.Tasks[index + 1]))
            {
                (Data.Tasks[index], Data.Tasks[index + 1]) = (Data.Tasks[index + 1], Data.Tasks[index]);
                successes++;
            }
        }

        // Finish
        ConcludeReordering(successes, selected);
    }

    private readonly ActionCommand taskMoveToTopCommand;
    public ActionCommand TaskMoveToTopCommand => taskMoveToTopCommand;

    private bool CanMoveTaskToTop(object? parameter)
        => Data.AtLeastOneSelected && !Data.SelectionHasMinimum;

    private void MoveTaskToTop(object? parameter)
    {
        // Snapshot and prepare
        var selected = GetSelection();
        List<RNTask> pushList = new(selected);
        int successes = 0, i;

        // Ignore elements already at target position
        for (i = 0; i < Data.Tasks.Count; i++)
        {
            if (pushList.Count <= 0)
                break;
            if (!Equals(Data.Tasks[i], pushList[0]))
            {
                // Remove duplicate of first push and insert it at current
                Data.Tasks.Remove(pushList[0]);
                Data.Tasks.Insert(i, pushList[0]);

                // Register success
                successes++;
            }
            //Remove current from pushList
            pushList.RemoveAt(0);
        }

        // Finish
        ConcludeReordering(successes, [.. selected]);
    }

    private readonly ActionCommand taskMoveToBottomCommand;
    public ActionCommand TaskMoveToBottomCommand => taskMoveToBottomCommand;

    private bool CanMoveTaskToBottom(object? parameter)
        => Data.AtLeastOneSelected && !Data.SelectionHasMaximum;

    private void MoveTaskToBottom(object? parameter)
    {
        // Snapshot and prepare
        var selected = GetSelection();
        List<RNTask> pushList = new(selected);
        int successes = 0, i;

        // Ignore elements already at target position
        for (i = 1; i <= Data.Tasks.Count; i++)
        {
            if (pushList.Count <= 0)
                break;
            if (!Equals(Data.Tasks[^i], pushList[^1]))
            {
                // Remove duplicate of first push and insert it at current
                Data.Tasks.Remove(pushList[^1]);
                // Need to +1, because insert on reverse direction should always be on the next item
                Data.Tasks.Insert(Data.Tasks.Count + 1 - i, pushList[^1]);

                // Register success
                successes++;
            }

            // Remove current from pushList
            pushList.RemoveAt(pushList.Count - 1);
        }

        // Finish
        ConcludeReordering(successes, [.. selected]);
    }

    // Commands : References
    private readonly ActionCommand referenceRemoveCommand;

    public ActionCommand ReferenceRemoveCommand => referenceRemoveCommand;

    private bool CanRemoveReference(object? parameter) => Data.AtLeastOneSelected;

    private void RemoveReference(object? parameter)
    {
        foreach (RNTask task in Data.Selection)
        {
            task.ReferencePath = string.Empty;
        }
    }

    private readonly ActionCommand referenceMoveUpCommand;
    public ActionCommand ReferenceMoveUpCommand => referenceMoveUpCommand;

    private bool CanMoveReferenceUp(object? parameter)
        => Data.AtLeastOneSelected && !Data.SelectionHasMinimum;

    private void MoveReferenceUp(object? parameter)
    {
        // Snapshot and prepare (No need to snapshot for references but it doesn't make a difference with this method anyway)
        var selected = GetSelection();
        int index, successes = 0;

        // enumerate: if index isn't minimum and previous item isn't also among selected, then swap references
        for (int i = 0; i < selected.Count; i++)
        {
            index = Data.Tasks.IndexOf(selected[i]);
            if (index != 0 && !selected.Contains(Data.Tasks[index - 1]))
            {
                (Data.Tasks[index].ReferencePath, Data.Tasks[index - 1].ReferencePath) = (Data.Tasks[index - 1].ReferencePath, Data.Tasks[index].ReferencePath);
                selected[i] = Data.Tasks[index - 1];
                successes++;
            }
        }

        // Conclude
        ConcludeReordering(successes, [.. selected]);
    }

    private readonly ActionCommand referenceMoveDownCommand;
    public ActionCommand ReferenceMoveDownCommand => referenceMoveDownCommand;

    private bool CanMoveReferenceDown(object? parameter)
        => Data.AtLeastOneSelected && !Data.SelectionHasMaximum;

    private void MoveReferenceDown(object? parameter)
    {
        // Snapshot and prepare (No need to snapshot for references but it doesn't make a difference with this method anyway)
        var selected = GetSelection();
        int index, successes = 0;

        // enumerate: if index isn't maximum and next item isn't also among selected, then swap references
        for (int i = selected.Count - 1; i >= 0; i--)
        {
            index = Data.Tasks.IndexOf(selected[i]);
            if (index != Data.Tasks.Count - 1 && !selected.Contains(Data.Tasks[index + 1]))
            {
                (Data.Tasks[index].ReferencePath, Data.Tasks[index + 1].ReferencePath) = (Data.Tasks[index + 1].ReferencePath, Data.Tasks[index].ReferencePath);
                selected[i] = Data.Tasks[index + 1];
                successes++;
            }
        }

        // Conclude
        ConcludeReordering(successes, [.. selected]);
    }

    private readonly ActionCommand referenceMoveToTopCommand;
    public ActionCommand ReferenceMoveToTopCommand => referenceMoveToTopCommand;

    private bool CanMoveReferenceToTop(object? parameter) => Data.AtLeastOneSelected;

    private void MoveReferenceToTop(object? parameter)
    {
        // Prepare
        RNTask[] selected = [.. GetSelection()];
        List<RNTask> newSelection = [];
        List<string> pushList = [];
        int offset = 0, selectedPushCount = 0, successes = 0;

        // Get first start position (skip over items that don't need to be moved, add them to new selection)
        for (int i = 0; i < Data.Tasks.Count; i++)
        {
            if (!selected.Contains(Data.Tasks[i]))
                break;
            newSelection.Add(Data.Tasks[i]);
            offset++;
        }

        // Move selected references out of the source
        for (int i = offset; i < Data.Tasks.Count; i++)
        {
            if (selected.Contains(Data.Tasks[i]))
            {
                pushList.Add(Data.Tasks[i].ReferencePath);
                Data.Tasks[i].ReferencePath = string.Empty;
                selectedPushCount++;
            }
        }

        // Start pushing down from startPos
        while (pushList.Count > 0)
        {
            // push to pushList if not refpath is not empty
            if (!string.IsNullOrEmpty(Data.Tasks[offset].ReferencePath))
                pushList.Add(Data.Tasks[offset].ReferencePath);

            // overwrite using first element in pushList
            Data.Tasks[offset].ReferencePath = pushList[0];
            pushList.RemoveAt(0);

            // add to new selection if within selectedCount
            if (offset < selected.Length)
            {
                newSelection.Add(Data.Tasks[offset]);
                successes++;
            }

            offset++;
        }

        // Finish
        ConcludeReordering(successes, [.. newSelection]);
    }

    private readonly ActionCommand referenceMoveToBottomCommand;
    public ActionCommand ReferenceMoveToBottomCommand => referenceMoveToBottomCommand;

    private bool CanMoveReferenceToBottom(object? parameter) => Data.AtLeastOneSelected;

    private void MoveReferenceToBottom(object? parameter)
    {
        RNTask[] selected = [.. GetSelection()];
        List<RNTask> newSelection = [];
        List<string> pushList = [];
        int i, offset = 0, selectedPushCount = 0, successes = 0;

        // Get first start position (skip over items that don't need to be moved, add them to new selection)
        for (i = Data.Tasks.Count - 1; i >= 0; i--)
        {
            if (!selected.Contains(Data.Tasks[i]))
                break;
            newSelection.Insert(0, Data.Tasks[i]);
            offset++;
        }

        // Move selected references out of the source
        for (i = Data.Tasks.Count - offset - 1; i >= 0; i--)
        {
            if (selected.Contains(Data.Tasks[i]))
            {
                pushList.Insert(0, Data.Tasks[i].ReferencePath);
                Data.Tasks[i].ReferencePath = string.Empty;
                selectedPushCount++;
            }
        }

        // Start pushing up from startPos
        i = offset + 1;
        while (pushList.Count > 0)
        {
            // push to pushList if not refpath is not empty
            if (!string.IsNullOrEmpty(Data.Tasks[^i].ReferencePath))
                pushList.Insert(0, Data.Tasks[^i].ReferencePath);

            // overwrite using last element in pushList
            Data.Tasks[^i].ReferencePath = pushList[^1];
            pushList.RemoveAt(pushList.Count - 1);

            // add to new selection if within selectedCount
            if (newSelection.Count < selected.Length)
            {
                newSelection.Insert(0, Data.Tasks[^i]);
                successes++;
            }

            i++;
        }

        // Finish
        ConcludeReordering(successes, [.. newSelection]);
    }

    // Commands : Rename
    private readonly ActionCommand renameAllCommand;

    public ActionCommand RenameAllCommand => renameAllCommand;

    private bool CanRenameAll(object? parameter) => Data.AtLeastOneTask; // Create AtleastOneTaskReady

    private void RenameAll(object? parameter)
        => RenameFiles(Data.Tasks.Where(x => x.Ready));

    private readonly ActionCommand renameSelectedCommand;
    public ActionCommand RenameSelectedCommand => renameSelectedCommand;

    private bool CanRenameSelected(object? parameter) => Data.AtLeastOneSelected; // Use AtleastOneSelected + Ready

    private void RenameSelected(object? parameter)
        => RenameFiles(Data.Selection.Where(x => x.Ready));

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

    // LifeTime
    public RNLogic() : this(new Dummy()) { }

    public RNLogic(IHandler handler)
    {
        // Handler
        _handler = handler;

        // Validations
        Data.TasksChanged += Data_TasksChanged;
        Data.SelectionChanged += Data_SelectionChanged;

        // Commands : Task
        taskRemoveCommand = new(RemoveTask, CanRemoveTask);

        taskMoveUpCommand = new(MoveTaskUp, CanMoveTaskUp);
        taskMoveDownCommand = new(MoveTaskDown, CanMoveTaskDown);
        taskMoveToTopCommand = new(MoveTaskToTop, CanMoveTaskToTop);
        taskMoveToBottomCommand = new(MoveTaskToBottom, CanMoveTaskToBottom);

        // Commands : Reference
        referenceRemoveCommand = new(RemoveReference, CanRemoveReference);

        referenceMoveUpCommand = new(MoveReferenceUp, CanMoveReferenceUp);
        referenceMoveDownCommand = new(MoveReferenceDown, CanMoveReferenceDown);
        referenceMoveToTopCommand = new(MoveReferenceToTop, CanMoveReferenceToTop);
        referenceMoveToBottomCommand = new(MoveReferenceToBottom, CanMoveReferenceToBottom);

        // Commands : Rename
        renameAllCommand = new(RenameAll, CanRenameAll);
        renameSelectedCommand = new(RenameSelected, CanRenameSelected);

        // Commands : Manage
        evaluateErrorsCommand = new(EvaluateErrors, CanEvaluateErrors);
        retaskCommand = new(Retask, CanRetask);

        // Commands : Purge (Add can-execute, atleast one task)
        clearCompletedCommand = new(ClearCompleted, CanClearCompleted);
        clearMissingCommand = new(ClearMissing, CanClearMissing);
        clearUnreadyCommand = new(ClearUnready, CanClearUnready);
        clearAllCommand = new(ClearAll, CanClearAll);
    }

    public void OnInterfaceLoaded()
    {
        LogAdd("Program loaded.");
        LogAdd("How-To basics:\n1. Add tasks - Files to be renamed,\n2. Append references - Files to copy names from,\n3. Apply rename.", "notes");
    }

    // Base Functions
    private void RenameFiles(IEnumerable<RNTask> tasks)
    {
        int count = 0, successes = 0, errors = 0;
        foreach (RNTask task in tasks)
        {
            count++;

            try
            {
                // Attempt rename
                Handler.FileSystemAgent.MoveFile(task.OriginalPath, task.PreviewPath);

                // On success
                task.SuccessStatus = true;
                LogAdd($"Renamed: \"{task.OriginalFilename}\" => \"{task.PreviewFilename}\" in {task.OriginalDirectory}", "details");
                successes++;

                // Cleanup
                task.OriginalPath = task.PreviewPath;
                task.ReferencePath = string.Empty;

                // If retask
                if (Data.AutoRetask)
                {
                    task.SuccessStatus = null;
                }
            }
            catch (Exception ex)
            {
                // On fail
                task.SuccessStatus = false;
                task.LastException = ex;
                LogAdd($"Renaming failed: \"{task.OriginalPath}\" => \"{task.PreviewPath}\"\nReason:\n{ex.Message}", "error");
                errors++;
            }
        }

        LogAdd($"{successes} of {count} files were renamed. Errors: {errors}.");
    }

    // Move this to the model and change to property? (Data.Selection replacement?)
    private List<RNTask> GetSelection()
    {
        List<RNTask> result = [];
        for (int i = 0; i < Data.Tasks.Count; i++)
        {
            if (Data.Tasks[i].IsSelected)
                result.Add(Data.Tasks[i]);
        }
        return result;
    }

    private void ConcludeReordering(int successes, RNTask[] selected)
    {
        // Beep if nothing was moved
        if (successes == 0)
            Handler.PromptAgent?.PlaySound("beep");

        // Restore selection
        else
        {
            // Force a reset to register the change
            foreach (var task in Data.Tasks)
                task.IsSelected = false;
            foreach (var task in selected)
                task.IsSelected = true;
        }
    }

    // Direct Functions
    public bool IsExitReady()
    => !Data.Tasks.Any(x => x.SuccessStatus != true);

    public void AddTasks(string[] paths, bool insert = false)
    {
        // Validate
        if (Invalid(paths) || (insert && InvalidSelection())) return;

        // Prepare
        int originalSelectedIndex = Data.SelectedIndex;
        int successes = 0, invalids = 0, absentees = 0, repeats = 0;
        string s;

        // Enumerate
        foreach (string file in paths)
        {
            s = Handler.FileSystemAgent.GetFullPath(file);

            // Duplicate check
            if (Data.Tasks.Any(x => x.OriginalPath == s))
            {
                LogAdd("Unable to add task (Already on the list): " + Handler.FileSystemAgent.GetFullPath(file));
                repeats++;
                continue;
            }

            // Bad Uri check
            if (string.IsNullOrWhiteSpace(s))
            {
                LogAdd("Unable to add task (Bad URI): " + s);
                invalids++;
                continue;
            }

            // Validity check
            if (!Handler.FileSystemAgent.FileExists(s))
            {
                LogAdd("Unable to add task (File does not exist): " + s);
                absentees++;
                continue;
            }

            // Add/Insert actual
            if (insert)
                Data.Tasks.Insert(originalSelectedIndex + successes, new(Handler.FileSystemAgent) { OriginalPath = s });
            else
                Data.Tasks.Add(new RNTask(Handler.FileSystemAgent) { OriginalPath = s });

            // Append status for log
            successes++;
        }

        // Log operation
        int n = paths.Length;
        LogAdd(
            $"{successes} of {n} files were {(insert ? "inserted in" : "added ")}to the task list."
            + (n - successes > 0 ? $" {repeats} duplicates, {invalids} bad Uri and {absentees} missing files were not added." : "")
        );
    }

    public void AddReferences(string[] paths, AddReferencesMode mode = AddReferencesMode.Append)
    {
        switch (mode)
        {
            case AddReferencesMode.ReplaceAt:
                ReplaceReferencesAt(paths); break;
            case AddReferencesMode.ReplaceAll:
                ReplaceAllReferences(paths); break;
            default:
                AppendReference(paths); break;
        };
    }

    private void AppendReference(string[] paths)
    {
        // Validate
        if (Invalid(paths)) return;

        // Check status
        var unreferenced = Data.Tasks.Where(x => string.IsNullOrWhiteSpace(x.ReferencePath)).ToArray();
        var uncount = unreferenced.Length;

        // Abort if there are no unreferenced tasks to add to
        if (uncount == 0)
        {
            string emsg = "Unable to add more references as there are no unreferenced tasks remaining."
                + " To add more references, either add more tasks, or clear references from pending tasks.";
            Handler.PromptAgent.Alert(emsg, "Nothing to reference", "error");
            LogAdd(emsg, "error");
            return;
        }

        // Prepare
        // (Exists validation is not required because only the filename is needed)
        // (Uniqueness validation condition: only for same target folder. Implementation pending)
        var fcount = paths.Length;
        int n = 0;

        for (int i = 0; i < fcount; i++)
        {
            if (i >= uncount)
            {
                break;
            }
            try
            {
                unreferenced[i].ReferencePath = Handler.FileSystemAgent.GetFullPath(paths[i]);
                n++;
            }
            catch (Exception x)
            {
                LogAdd($"Error - {x.Source}: {x.Message}");
            }
        }

        // Log count mismatch message
        LogAdd($"Added {n} of {fcount} name references."
            + (uncount > fcount ? $" {uncount - fcount} unreferenced tasks remaining. Add more reference files to continue from the next unreferenced task." : "")
            + (uncount < fcount ? $" {fcount - uncount} excess references files were ignored. If this was not intended, clear reference(s) and try again." : "")
            );
    }

    private void ReplaceReferencesAt(string[] paths)
    {
        // Validate
        if (Invalid(paths) || InvalidTasks()) return;
        if (Data.SelectedIndex == -1)
        {
            LogAdd("Nothing is selected. Unable to proceed.", "information");
            return;
        }

        // Process
        int i = 0;
        int j = Data.SelectedIndex;
        int k = paths.Length;
        int l = Data.Tasks.Count;

        while (j + i < l && i < k)
        {
            Data.Tasks[j + i].ReferencePath = paths[i];
            i++;
        }

        // Post process
        LogAdd($"Added {i} of {k} name references added."
            + (i < k ? $" {k - i} excess remaining. Reached end of tasks list. Unable to continue without adding more tasks." : "")
            );
    }

    private void ReplaceAllReferences(string[] paths)
    {
        // Validate
        if (Invalid(paths) || InvalidTasks()) return;

        // Process
        int i;
        int l = paths.Length;
        int c = Data.Tasks.Count;
        int n = l < c ? l : c;
        for (i = 0; i < n; i++)
        {
            Data.Tasks[i].ReferencePath = paths[i];
        }

        // Post
        LogAdd($"Added {i} of {l} name references."
            + (c > l ? $" {c - l} unreferenced tasks remaining. Add more reference files to continue from the next unreferenced task." : "")
            + (c < l ? $" {l - c} excess references files were ignored. If this was not intended, clear reference(s) and try again." : "")
            );
    }

    private bool Invalid(string[] paths)
    {
        bool result = paths.Length > 0;
        if (result)
            LogAdd("No files were provided. Possible internal error.", "information");
        return result;
    }

    private bool InvalidTasks()
    {
        bool result = Data.Tasks.Count < 1;
        if (result)
            LogAdd("There are no tasks. Add some tasks and try again.", "information");
        return result;
    }

    private bool InvalidSelection()
    {
        bool result = Data.SelectedIndex == -1;
        if (result)
            LogAdd("Nothing is selected. Select a task and try again.", "information");
        return result;
    }
}