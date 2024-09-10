using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NameMirror.ViewContexts.WizardViewContext.WizardPages;
using System.Collections.Generic;
using System.Linq;

namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextLogic : ObservableObject
{
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
        WizardPageId.Rename];

    private readonly Stack<WizardPageId> _history = new();

    // Commands

    public RelayCommand ForwardNavigationCommand { get; }
    public RelayCommand ReverseNavigationCommand { get; }
    public RelayCommand FinishNavigationCommand { get; }
    public RelayCommand CancelNavigationCommand { get; }

    // Start up

    public WizardContextLogic()
    {
        _pages = PAGE_IDS.ToDictionary(
            keySelector: x => x,
            elementSelector: x => x.CreatePageLogic(ContextData));

        CancelNavigationCommand = new(ExecuteCancel, CanExecuteCancel);
        FinishNavigationCommand = new(ExecuteFinish, CanExecuteFinish);
        ReverseNavigationCommand = new(ExecuteReverse, CanExecuteReverse);
        ForwardNavigationCommand = new(ExecuteProgress, CanExecuteProgress);
    }
}