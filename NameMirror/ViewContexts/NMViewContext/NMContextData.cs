using CommunityToolkit.Mvvm.ComponentModel;
using NameMirror.Types;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NameMirror.ViewContexts.NMViewContext;

public partial class NMContextData : ObservableObject
{
    public bool IsBusy { get; set; } = false;

    [ObservableProperty]
    private int _selectedIndex = 0;

    [ObservableProperty]
    private bool _autoRetask = false;

    // Validation

    [ObservableProperty]
    private bool _atLeastOneTask = false;

    [ObservableProperty]
    private bool _atLeastOneSelected = false;

    [ObservableProperty]
    private bool _selectionHasMinimum = false;

    [ObservableProperty]
    private bool _selectionHasMaximum = false;

    // Ctor

    public NMContextData()
    {
        Tasks.CollectionChanged += Tasks_CollectionChanged;
        Selection.CollectionChanged += Selection_CollectionChanged;
    }

    // Tasks

    public event EventHandler? TasksChanged;

    private readonly ObservableCollection<RNTask> tasks = [];
    public ObservableCollection<RNTask> Tasks => tasks;

    private void Tasks_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        AtLeastOneTask = Tasks.Count > 0;
        SelectionHasMinimum = EvaluateMinimumIndex();
        SelectionHasMaximum = EvaluateMaximumIndex();
        TasksChanged?.Invoke(this, new());
    }

    // Selection

    public event EventHandler? SelectionChanged;

    private readonly ObservableCollection<RNTask> selected = [];
    public ObservableCollection<RNTask> Selection => selected;

    private void Selection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        AtLeastOneSelected = Selection.Count > 0;
        SelectionHasMinimum = EvaluateMinimumIndex();
        SelectionHasMaximum = EvaluateMaximumIndex();
        SelectionChanged?.Invoke(this, new());
    }

    public bool EvaluateMinimumIndex()
        => Selection.Any(x => Tasks.IndexOf(x) == 0);

    public bool EvaluateMaximumIndex()
        => Selection.Any(x => Tasks.IndexOf(x) == Tasks.Count - 1);
}