namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextLogic
{
    // Properties

    public bool CanCancel => CurrentPage.CanCancel();
    public bool CanFinish => CurrentPage.CanFinish();
    public bool CanReverse => CurrentPage.CanReverse();
    public bool CanProgress => CurrentPage.CanProgress();

    // Cancel

    private bool CanExecuteCancel()
        => CanCancel;

    private void ExecuteCancel()
    {
        if (CurrentPage.Cancel())
            Shutdown();
    }

    // Finish

    private bool CanExecuteFinish()
        => CurrentPage.CanFinish();

    private void ExecuteFinish()
        => Shutdown(true);

    // Reverse

    private bool CanExecuteReverse()
        => CanReverse;

    private void ExecuteReverse()
    {
        WizardPageId previous
            = CurrentPage.Reverse()
            ?? (_history.Count > 0 ? _history.Pop() : WizardPageId.Start);

        SetPage(previous);
    }

    // Forward

    private bool CanExecuteProgress()
        => CanProgress;

    private void ExecuteProgress()
    {
        WizardPageId next = CurrentPage.Progress();
        _history.Push(next);

        SetPage(next);
    }
}