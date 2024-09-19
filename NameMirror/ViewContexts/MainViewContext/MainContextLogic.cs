using CommunityToolkit.Mvvm.ComponentModel;
using NameMirror.Types;
using NameMirror.ViewContexts.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic : ObservableObject, IRenameTaskReceptor
{
    // Components
    [ObservableProperty]
    private MainContextData _data = new();

    private readonly NameMirrorServices _services;

    private void Log(string message, string level = "information", object? sender = null)
        => _services.LogService.Log(level, message, sender ?? this);

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
    public MainContextLogic()
    {
        _services = NameMirrorServices.Current;
        _services.MainContextTaskReceptor = this;

        // Validations
        Data.TasksChanged += Data_TasksChanged;
        Data.SelectionChanged += Data_SelectionChanged;

        // Commands : Targets
        AddTargetsCommand = new(ExecuteAddTargets);
        InsertTargetsCommand = new(ExecuteInsertTargets, CanExecuteInsertTargets);

        TaskRemoveCommand = new(RemoveTask, CanRemoveTask);

        TaskMoveUpCommand = new(MoveTaskUp, CanMoveTaskUp);
        TaskMoveDownCommand = new(MoveTaskDown, CanMoveTaskDown);
        TaskMoveToTopCommand = new(MoveTaskToTop, CanMoveTaskToTop);
        TaskMoveToBottomCommand = new(MoveTaskToBottom, CanMoveTaskToBottom);

        // Commands : Reference
        AppendReferencesCommand = new(ExecuteAppendReferences);
        ReplaceReferencesAtCommand = new(ExecuteReplaceReferencesAt, CanExecuteReplaceReferencesAt);
        ReplaceAllReferencesCommand = new(ExecuteReplaceAllReferences);

        ReferenceRemoveCommand = new(RemoveReference, CanRemoveReference);

        ReferenceMoveUpCommand = new(MoveReferenceUp, CanMoveReferenceUp);
        ReferenceMoveDownCommand = new(MoveReferenceDown, CanMoveReferenceDown);
        ReferenceMoveToTopCommand = new(MoveReferenceToTop, CanMoveReferenceToTop);
        ReferenceMoveToBottomCommand = new(MoveReferenceToBottom, CanMoveReferenceToBottom);

        // Commands : Rename
        RenameAllCommand = new(RenameAll, CanRenameAll);
        RenameSelectedCommand = new(RenameSelected, CanRenameSelected);

        // Commands : Manage
        EvaluateErrorsCommand = new(EvaluateErrors, CanEvaluateErrors);
        RetaskCommand = new(Retask, CanRetask);

        // Commands : Purge (Add can-execute, atleast one task)
        ClearCompletedCommand = new(ClearCompleted, CanClearCompleted);
        ClearMissingCommand = new(ClearMissing, CanClearMissing);
        ClearUnreadyCommand = new(ClearUnready, CanClearUnready);
        ClearAllCommand = new((_) => ClearAll(), CanClearAll);
    }

    public void OnInterfaceLoaded()
    {
        Log("Program loaded.");
        Log("How-To basics:\n1. Add tasks - Files to be renamed,\n2. Append references - Files to copy names from,\n3. Apply rename.", "notes");
    }

    // Interface : IRenameTaskReceptor

    public void SetTasks(IList<RenameTask> tasks, bool clearExisting = false)
    {
        if (clearExisting)
            ClearAll(confirm: false);

        AddTasks(tasks);
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
                File.Move(task.OriginalPath, task.PreviewPath);

                // On success
                task.SuccessStatus = true;
                Log($"Renamed: \"{task.OriginalFilename}\" => \"{task.PreviewFilename}\" in {task.OriginalDirectory}", "details");
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
                Log($"Renaming failed: \"{task.OriginalPath}\" => \"{task.PreviewPath}\"\nReason:\n{ex.Message}", "error");
                errors++;
            }
        }

        Log($"{successes} of {count} files were renamed. Errors: {errors}.");
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
            _services.PromptAgent.PlaySound("beep");

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

    private bool Invalid(string[] paths)
    {
        bool result = paths.Length > 0;
        if (result)
            Log("No files were provided. Possible internal error.", "information");
        return result;
    }

    private bool InvalidTasks()
    {
        bool result = Data.Tasks.Count < 1;
        if (result)
            Log("There are no tasks. Add some tasks and try again.", "information");
        return result;
    }

    private bool InvalidSelection()
    {
        bool result = Data.SelectedIndex == -1;
        if (result)
            Log("Nothing is selected. Select a task and try again.", "information");
        return result;
    }
}