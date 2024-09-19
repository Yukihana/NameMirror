using NameMirror.ViewContexts.WizardViewContext.WizardPages;
using System;

namespace NameMirror.ViewContexts.WizardViewContext;

public static partial class WizardExtensions
{
    public static IWizardPage CreatePageLogic(this WizardPageId pageId, IWizardData data) => pageId switch
    {
        WizardPageId.Start => new PageStart(data),
        WizardPageId.AddTargets => new PageAddTargets(data),
        WizardPageId.AddReferences => new PageAddReferences(data),
        WizardPageId.Review => new PageReview(data),
        WizardPageId.Choice => new PageChoice(data),
        WizardPageId.Rename => new PageRename(data),
        WizardPageId.FinalizeForEdit => new PageFinalizeForEdit(data),
        _ => throw new InvalidOperationException($"Unsupported page Id: {pageId}")
    };

    public static int ToRecommendedPageIndex(this WizardPageId pageId) => pageId switch
    {
        WizardPageId.Start => 0,
        WizardPageId.AddTargets => 1,
        WizardPageId.AddReferences => 2,
        WizardPageId.Review => 3,
        WizardPageId.Choice => 4,
        WizardPageId.Rename => 5,
        WizardPageId.FinalizeForEdit => 6,
        _ => throw new InvalidOperationException($"Unsupported page Id: {pageId}"),
    };
}