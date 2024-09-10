namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageStart(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.Start;

    public IWizardData Data { get; } = data;

    // Functions

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

    public bool CanReverse() => false;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => true;

    public WizardPageId Progress() => WizardPageId.AddTargets;
}