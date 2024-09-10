using NameMirror.ViewContexts.Shared;
using System.Collections.ObjectModel;

namespace NameMirror.ViewContexts.WizardViewContext;

public interface IWizardData
{
    ObservableCollection<FilePath> Targets { get; }
    ObservableCollection<FilePath> References { get; }
    ObservableCollection<RenameTask> RenameTasks { get; }
    bool PushToEdit { get; }
    bool RenameCompleted { get; }
    bool RenameStarted { get; }
}