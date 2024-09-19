using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageFinalizeForEdit(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.Choice;

    public IWizardData Data { get; } = data;

    // Load

    public object? PreLoad()
    {
        Data.ProgressMode = WizardProgressMode.Finish;
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

    // Cancel

    public bool CanCancel() => true;

    public bool Cancel() => true;

    // Reverse

    public bool CanReverse() => true;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress()
        => Data.IsEditOptionSelected || Data.IsEditOptionSelected;

    public WizardPageId Progress()
    {
        if (!CanProgress())
            throw new InvalidOperationException();

        if (Data.IsRenameOptionSelected)
            return WizardPageId.Rename;
        throw new NotImplementedException();
    }
}