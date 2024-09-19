using System;
using System.Threading.Tasks;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageChoice(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.Choice;

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
        Data.ProgressMode
            = Data.IsRenameOptionSelected
            ? WizardProgressMode.Confirm
            : WizardProgressMode.Next;
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

    public bool CanProgress()
        => Data.IsRenameOptionSelected || Data.IsEditOptionSelected;

    public WizardPageId? Progress()
    {
        if (Data.IsEditOptionSelected)
            return WizardPageId.FinalizeForEdit;
        if (Data.IsRenameOptionSelected)
            return WizardPageId.Rename;
        throw new InvalidOperationException();
    }
}