using System;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageRename(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.Rename;

    public IWizardData Data { get; } = data;

    public void Load()
    {
    }

    // Cancel

    public bool CanCancel() => true;

    public bool Cancel() => true;

    // Finish

    public bool CanFinish() => false; // Send update notif after rename is completed then change this to true.

    public bool Finish() => Data.RenameCompleted;

    // Reverse

    public bool CanReverse() => !(Data.RenameStarted || Data.RenameCompleted);

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => false;

    public WizardPageId Progress() => throw new NotImplementedException();
}