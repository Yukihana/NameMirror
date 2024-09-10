using System;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageChoice(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.Choice;

    public IWizardData Data { get; } = data;

    public void Load()
    {
    }

    // Cancel

    public bool CanCancel() => true;

    public bool Cancel() => true;

    // Finish

    public bool CanFinish() => false;

    public bool Finish() => false;

    // Reverse

    public bool CanReverse() => true;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => !Data.PushToEdit; // No if edit is selected.

    public WizardPageId Progress()
    {
        if (Data.PushToEdit)
            throw new InvalidOperationException();

        throw new NotImplementedException();
    }
}