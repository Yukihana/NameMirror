﻿using System.Linq;
using System.Threading.Tasks;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageAddTargets(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.AddTargets;

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

    // Cancel

    public bool CanCancel() => true;

    public bool Cancel() => true;

    // Reverse

    public bool CanReverse() => true;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => Data.Targets.Any();

    public WizardPageId Progress() => WizardPageId.AddReferences;
}