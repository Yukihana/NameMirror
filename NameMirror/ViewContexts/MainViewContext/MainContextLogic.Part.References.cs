using NameMirror.Commands;
using NameMirror.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
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
}