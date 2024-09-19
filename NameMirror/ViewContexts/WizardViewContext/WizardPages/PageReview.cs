using CSX.DotNet.Shared.Threading;
using NameMirror.ViewContexts.Shared;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageReview(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.Review;

    public IWizardData Data { get; } = data;

    // Load

    public object? PreLoad()
    {
        Data.ProgressMode = WizardProgressMode.Next;
        if (Data.Targets.Count != Data.References.Count)
            Data.IsReviewNoticeHidden = false;
        return null;
    }

    public async Task<object?> Load(object? state)
    {
        await Task.Yield();
        var combined = Data.Targets.Zip(Data.References, (x, y) => new RenameTask(x, y));
        ObservableCollection<RenameTask> tasks = new(combined);
        return tasks;
    }

    public void PostLoad(object? state)
    {
        Data.RenameTasks
            = state as ObservableCollection<RenameTask>
            ?? throw new ArgumentException();
    }

    // Cancel

    public bool CanCancel() => true;

    public bool Cancel() => true;

    // Reverse

    public bool CanReverse() => true;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => true; // Add confirmation dialog for target/reference count non-equivalency.

    public WizardPageId Progress() => WizardPageId.Choice;
}