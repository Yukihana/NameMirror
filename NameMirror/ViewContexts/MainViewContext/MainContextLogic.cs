using CommunityToolkit.Mvvm.ComponentModel;
using NameMirror.Agents;
using NameMirror.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic : ObservableObject
{
    // Components
    [ObservableProperty]
    private MainContextData _data = new();

    //    public event PropertyChangedEventHandler? PropertyChanged;

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

    // LifeTime
    public MainContextLogic() : this(new Dummy()) { }

    public MainContextLogic(IHandler handler)
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