using NameMirror.Commands;
using NameMirror.Types;
using System.Collections.Generic;
using System.Linq;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
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
}