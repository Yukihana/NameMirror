using CommunityToolkit.Mvvm.ComponentModel;
using NameMirror.ViewContexts.Shared;
using System.Collections.ObjectModel;

namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextData : ObservableObject, IWizardData
{
    [ObservableProperty]
    private ObservableCollection<FilePath> _targets = [];

    [ObservableProperty]
    private ObservableCollection<FilePath> _references = [];

    [ObservableProperty]
    private ObservableCollection<RenameTask> _renameTasks = [];

    [ObservableProperty]
    public bool _pushToEdit = false;

    [ObservableProperty]
    public bool _renameCompleted = false;

    [ObservableProperty]
    public bool _renameStarted = false;
}