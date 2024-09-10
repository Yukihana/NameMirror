using System;
using System.Linq;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageAddReferences(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.AddReferences;

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

    public bool Finish() => throw new NotImplementedException();

    // Reverse

    public bool CanReverse() => true;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => Data.References.Any();

    public WizardPageId Progress() => WizardPageId.Review;
}