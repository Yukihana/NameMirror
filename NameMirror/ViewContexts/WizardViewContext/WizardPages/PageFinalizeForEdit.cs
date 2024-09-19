using NameMirror.ViewContexts.MainViewContext;
using NameMirror.ViewContexts.Shared;
using System;
using System.Collections.Generic;
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

    public bool CanProgress()
        => Data.IsEditOptionSelected || Data.IsEditOptionSelected;

    public WizardPageId? Progress()
    {
        try
        {
            if (NameMirrorServices.Current.MainContextTaskReceptor is not IRenameTaskReceptor receptor)
                throw new InvalidOperationException("MainContext task receptor interface not found.");

            receptor.SetTasks(
                tasks: new List<RenameTask>(Data.RenameTasks),
                clearExisting: Data.ClearBeforeEdit);
        }
        catch (Exception ex)
        {
            NameMirrorServices.Current.LogService.Log("error", "MainContext task receptor interface not found.", this, ex);
        }
        return null;
    }
}