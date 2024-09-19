using CommunityToolkit.Mvvm.ComponentModel;
using NameMirror.ViewContexts.Shared;
using NameMirror.ViewContexts.WizardViewContext.WizardPages;
using System.Collections.ObjectModel;

namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextData : ObservableObject, IWizardData
{
    // Data

    [ObservableProperty]
    private ObservableCollection<FilePath> _targets = [];

    [ObservableProperty]
    private ObservableCollection<FilePath> _references = [];

    [ObservableProperty]
    private ObservableCollection<RenameTask> _renameTasks = [];

    [ObservableProperty]
    private ObservableCollection<string> _log = [];

    // Choices

    [ObservableProperty]
    private bool _isRenameOptionSelected = false;

    [ObservableProperty]
    private bool _isEditOptionSelected = false;

    // States

    [ObservableProperty]
    private bool _isBusy = false;

    [ObservableProperty]
    private bool _isReviewNoticeHidden = true;

    [ObservableProperty]
    private WizardProgressMode _progressMode = WizardProgressMode.Next;

    [ObservableProperty]
    private bool _renameCompleted = false;
}