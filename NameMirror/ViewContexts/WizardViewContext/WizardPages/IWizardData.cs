using NameMirror.ViewContexts.Shared;
using System.Collections.ObjectModel;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public interface IWizardData
{
    // Data

    ObservableCollection<FilePath> Targets { get; }
    ObservableCollection<FilePath> References { get; }
    ObservableCollection<RenameTask> RenameTasks { get; set; }
    string Log { get; set; }

    // Navigation

    bool IsBusy { get; }
    WizardProgressMode ProgressMode { get; set; }

    // States

    bool IsRenameOptionSelected { get; }
    bool IsEditOptionSelected { get; }
    bool ClearBeforeEdit { get; }

    // UI

    bool IsReviewNoticeHidden { get; set; }
    bool IsProgressIndeterminate { get; set; }
    double ProgressValue { get; set; }
    double ProgressMaximum { get; set; }
    bool RenameCompleted { get; }
}