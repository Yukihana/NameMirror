using CommunityToolkit.Mvvm.Input;
using NameMirror.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands

    public RelayCommand AppendReferencesCommand { get; }
    public RelayCommand ReplaceReferencesAtCommand { get; }
    public RelayCommand ReplaceAllReferencesCommand { get; }

    public RelayCommand ReferenceRemoveCommand { get; }

    public RelayCommand ReferenceMoveUpCommand { get; }
    public RelayCommand ReferenceMoveDownCommand { get; }
    public RelayCommand ReferenceMoveToTopCommand { get; }
    public RelayCommand ReferenceMoveToBottomCommand { get; }

    // Add

    private void ExecuteAppendReferences()
    {
        if (_services.FileInputService.AddFiles(FileInputReason.AppendReferences) is string[] paths)
            AppendReferences(paths);
        else
            Log("Cancelled adding references (Append)", "debug");
    }

    private void ExecuteReplaceReferencesAt()
    {
        if (_services.FileInputService.AddFiles(FileInputReason.ReplaceReferencesAt) is string[] paths)
            ReplaceReferencesAt(paths);
        else
            Log("Cancelled adding references (Replace At)", "debug");
    }

    private void ExecuteReplaceAllReferences()
    {
        if (_services.FileInputService.AddFiles(FileInputReason.ReplaceAllReferences) is string[] paths)
            ReplaceAllReferences(paths);
        else
            Log("Cancelled adding references (Replace All)", "debug");
    }

    private void AppendReferences(string[] paths)
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
            _services.PromptAgent.Alert(emsg, "Nothing to reference", "error");
            Log(emsg, "error");
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
                unreferenced[i].ReferencePath = Path.GetFullPath(paths[i]);
                n++;
            }
            catch (Exception x)
            {
                Log($"Error - {x.Source}: {x.Message}");
            }
        }

        // Log count mismatch message
        Log($"Added {n} of {fcount} name references."
            + (uncount > fcount ? $" {uncount - fcount} unreferenced tasks remaining. Add more reference files to continue from the next unreferenced task." : "")
            + (uncount < fcount ? $" {fcount - uncount} excess references files were ignored. If this was not intended, clear reference(s) and try again." : "")
            );
    }

    private bool CanExecuteReplaceReferencesAt()
        => Data.AtLeastOneSelected;

    private void ReplaceReferencesAt(string[] paths)
    {
        // Validate
        if (Invalid(paths) || InvalidTasks()) return;
        if (Data.SelectedIndex == -1)
        {
            Log("Nothing is selected. Unable to proceed.", "information");
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
        Log($"Added {i} of {k} name references added."
            + (i < k ? $" {k - i} excess remaining. Reached end of tasks list. Unable to continue without adding more tasks." : "")
            );
    }

    private bool CanReplaceAllReferences()
        => Data.AtLeastOneTask;

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
        Log($"Added {i} of {l} name references."
            + (c > l ? $" {c - l} unreferenced tasks remaining. Add more reference files to continue from the next unreferenced task." : "")
            + (c < l ? $" {l - c} excess references files were ignored. If this was not intended, clear reference(s) and try again." : "")
            );
    }

    // Remove

    private bool CanRemoveReference() => Data.AtLeastOneSelected;

    private void RemoveReference()
    {
        foreach (RNTask task in Data.Selection)
        {
            task.ReferencePath = string.Empty;
        }
    }

    private bool CanMoveReferenceUp()
        => Data.AtLeastOneSelected && !Data.SelectionHasMinimum;

    private void MoveReferenceUp()
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

    private bool CanMoveReferenceDown()
        => Data.AtLeastOneSelected && !Data.SelectionHasMaximum;

    private void MoveReferenceDown()
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

    private bool CanMoveReferenceToTop() => Data.AtLeastOneSelected;

    private void MoveReferenceToTop()
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

    private bool CanMoveReferenceToBottom() => Data.AtLeastOneSelected;

    private void MoveReferenceToBottom()
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
}