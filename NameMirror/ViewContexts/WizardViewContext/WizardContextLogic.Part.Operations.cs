namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextLogic
{
    private void SetPage(WizardPageId pageId)
    {
        CurrentPageId = pageId;
        CurrentPage.Load();
    }

    private void Shutdown(bool isFinish = false)
    {
    }
}