using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NameMirror.ViewContexts.WizardViewContext.WizardPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextLogic : ObservableObject
{
    // Services

    private readonly NameMirrorServices _services = NameMirrorServices.Current;

    // Data : Business

    [ObservableProperty]
    private WizardContextData _contextData = new();

    // Data : Navigation

    private readonly Dictionary<WizardPageId, IWizardPage> _pages;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentPage))]
    private WizardPageId _currentPageId = WizardPageId.Start;

    public IWizardPage CurrentPage => _pages[CurrentPageId];

    private static readonly WizardPageId[] PAGE_IDS = [
        WizardPageId.Start,
        WizardPageId.AddTargets,
        WizardPageId.AddReferences,
        WizardPageId.Review,
        WizardPageId.Choice,
        WizardPageId.Rename,
        WizardPageId.FinalizeForEdit,
    ];

    private readonly Stack<WizardPageId> _history = new();

    // Commands: Navigation

    public RelayCommand ForwardNavigationCommand { get; }
    public RelayCommand ReverseNavigationCommand { get; }
    public RelayCommand CancelNavigationCommand { get; }

    // Commands: Inputs

    public RelayCommand AddTargetFilesCommand { get; }
    public RelayCommand AddTargetFolderCommand { get; }
    public RelayCommand ClearTargetsCommand { get; }
    public RelayCommand AddReferenceFilesCommand { get; }
    public RelayCommand AddReferenceFolderCommand { get; }
    public RelayCommand ClearReferencesCommand { get; }

    // Commands: Others

    public RelayCommand OptionSelectedCommand { get; }

    // Start up

    public WizardContextLogic()
    {
        _pages = PAGE_IDS.ToDictionary(
            keySelector: x => x,
            elementSelector: x => x.CreatePageLogic(ContextData));

        CancelNavigationCommand = new(ExecuteCancel, CanExecuteCancel);
        ReverseNavigationCommand = new(ExecuteReverse, CanExecuteReverse);
        ForwardNavigationCommand = new(ExecuteProgress, CanExecuteProgress);

        AddTargetFilesCommand = new(AddTargetFiles);
        AddTargetFolderCommand = new(AddTargetFolder);
        ClearTargetsCommand = new(ExecuteClearTargets);

        AddReferenceFilesCommand = new(AddReferenceFiles);
        AddReferenceFolderCommand = new(AddReferenceFolder);
        ClearReferencesCommand = new(ExecuteClearReferences);

        OptionSelectedCommand = new(ExecuteOptionSelected);
    }

    // Exposed : Close (For View CodeBehind)

    public event EventHandler? RequestClose;

    public bool CanCloseView()
        => CanExecuteCancel();
}