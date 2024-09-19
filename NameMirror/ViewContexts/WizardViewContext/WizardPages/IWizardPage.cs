using System.Threading.Tasks;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public interface IWizardPage
{
    IWizardData Data { get; }

    WizardPageId PageId { get; }

    // Load

    object? PreLoad();

    Task<object?> Load(object? state);

    void PostLoad(object? state);

    // Update

    void Update();

    // Close

    bool CanClose();

    bool Close();

    // Cancel

    bool CanCancel();

    bool Cancel();

    // Reverse

    bool CanReverse();

    WizardPageId? Reverse();

    // Progress

    bool CanProgress();

    WizardPageId? Progress();
}