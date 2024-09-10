namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public interface IWizardPage
{
    IWizardData Data { get; }

    WizardPageId PageId { get; }

    // Functions

    void Load();

    // Cancel

    bool CanCancel();

    bool Cancel();

    // Finish

    bool CanFinish();

    bool Finish();

    // Reverse

    bool CanReverse();

    WizardPageId? Reverse();

    // Progress

    bool CanProgress();

    WizardPageId Progress();
}