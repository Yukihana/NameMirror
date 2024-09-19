using System.Linq;
using System.Threading.Tasks;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageAddReferences(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.AddReferences;

    public IWizardData Data { get; } = data;

    // Load

    public object? PreLoad()
    {
        Data.ProgressMode = WizardProgressMode.Next;
        return null;
    }

    public async Task<object?> Load(object? state)
    {
        await Task.Yield();
        return null;
    }

    public void PostLoad(object? state)
    {
    }

    // Update

    public void Update()
    {
    }

    // Close

    public bool CanClose() => true;

    public bool Close() => true;

    // Cancel

    public bool CanCancel() => true;

    public bool Cancel() => true;

    // Reverse

    public bool CanReverse() => true;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => Data.References.Any();

    public WizardPageId? Progress() => WizardPageId.Review;
}