using ReferencedNaming.Types;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ReferencedNaming.Models
{
    public class RNData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsBusy { get; set; } = false;

        public RNData()
        {
            Tasks.CollectionChanged += Tasks_CollectionChanged;
            Selection.CollectionChanged += Selection_CollectionChanged;
        }

        // Data
        public event EventHandler? TasksChanged;

        private readonly ObservableCollection<RNTask> tasks = new();
        public ObservableCollection<RNTask> Tasks => tasks;

        private void Tasks_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AtLeastOneTask = Tasks.Count > 0;
            SelectionHasMinimum = EvaluateMinimumIndex();
            selectionHasMaximum = EvaluateMaximumIndex();
            TasksChanged?.Invoke(this, new());
        }

        public event EventHandler? SelectionChanged;

        private readonly ObservableCollection<RNTask> selected = new();
        public ObservableCollection<RNTask> Selection => selected;

        private void Selection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AtLeastOneSelected = Selection.Count > 0;
            SelectionHasMinimum = EvaluateMinimumIndex();
            selectionHasMaximum = EvaluateMaximumIndex();
            SelectionChanged?.Invoke(this, new());
        }

        private int _selectedIndex = 0;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    PropertyChanged?.Invoke(this, new(nameof(SelectedIndex)));
                }
            }
        }

        private bool _autoRetask = false;

        public bool AutoRetask
        {
            get => _autoRetask;
            set
            {
                if (_autoRetask != value)
                {
                    _autoRetask = value;
                    PropertyChanged?.Invoke(this, new(nameof(AutoRetask)));
                }
            }
        }

        // Validation
        private bool atLeastOneTask = false;

        public bool AtLeastOneTask
        {
            get => atLeastOneTask;
            set
            {
                if (atLeastOneTask != value)
                {
                    atLeastOneTask = value;
                    PropertyChanged?.Invoke(this, new(nameof(AtLeastOneTask)));
                }
            }
        }

        private bool atLeastOneSelected = false;

        public bool AtLeastOneSelected
        {
            get => atLeastOneSelected;
            set
            {
                if (atLeastOneSelected != value)
                {
                    atLeastOneSelected = value;
                    PropertyChanged?.Invoke(this, new(nameof(AtLeastOneSelected)));
                }
            }
        }

        private bool selectionHasMinimum;

        public bool SelectionHasMinimum
        {
            get => selectionHasMinimum;
            set
            {
                selectionHasMinimum = value;
            }
        }

        public bool EvaluateMinimumIndex()
        {
            foreach (RNTask task in Selection)
            {
                if (Tasks.IndexOf(task) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool selectionHasMaximum;

        public bool SelectionHasMaximum
        {
            get => selectionHasMaximum;
            set
            {
                selectionHasMaximum = value;
            }
        }

        public bool EvaluateMaximumIndex()
        {
            foreach (RNTask task in Selection)
            {
                if (Tasks.IndexOf(task) == Tasks.Count - 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}