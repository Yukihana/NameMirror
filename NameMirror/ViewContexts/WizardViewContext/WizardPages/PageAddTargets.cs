using System.Linq;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageAddTargets(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.AddTargets;

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

    public bool CanReverse() => true;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => Data.Targets.Any();

    public WizardPageId Progress() => WizardPageId.AddReferences;
}