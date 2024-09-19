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
    private string _log = string.Empty;

    // Navigation

    [ObservableProperty]
    private bool _isBusy = false;

    [ObservableProperty]
    private WizardProgressMode _progressMode = WizardProgressMode.Next;

    // Choices

    [ObservableProperty]
    private bool _isRenameOptionSelected = false;

    [ObservableProperty]
    private bool _isEditOptionSelected = false;

    [ObservableProperty]
    private bool _clearBeforeEdit = false;

    // UI

    [ObservableProperty]
    private bool _isReviewNoticeHidden = true;

    [ObservableProperty]
    private bool _isProgressIndeterminate = false;

    [ObservableProperty]
    private double _progressValue = 0;

    [ObservableProperty]
    private double _progressMaximum = 1;

    [ObservableProperty]
    private bool _renameCompleted = false;
}