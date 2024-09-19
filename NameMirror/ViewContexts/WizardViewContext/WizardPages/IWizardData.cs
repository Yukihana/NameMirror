using NameMirror.ViewContexts.Shared;
using System.Collections.ObjectModel;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public interface IWizardData
{
    // Data

    ObservableCollection<FilePath> Targets { get; }
    ObservableCollection<FilePath> References { get; }
    ObservableCollection<RenameTask> RenameTasks { get; set; }

    // Navigation

    WizardProgressMode ProgressMode { get; set; }

    // States

    bool IsBusy { get; }
    bool IsRenameOptionSelected { get; }
    bool IsEditOptionSelected { get; }
    bool IsReviewNoticeHidden { get; set; }
    bool RenameCompleted { get; }
}