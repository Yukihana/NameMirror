using NameMirror.ViewContexts.WizardViewContext.WizardPages;
using System.Threading.Tasks;

namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextLogic
{
    // Operations

    private void SetPage(WizardPageId pageId)
    {
        CurrentPageId = pageId;
        Task.Run(() => LoadPage(CurrentPage));
    }

    private async Task LoadPage(IWizardPage page)
    {
        object? preState = null;
        NameMirrorServices.Current.SynchronizationContext.Send((_) =>
        {
            ContextData.IsBusy = true;
            UpdateNavigation();

            preState = page.PreLoad();
        }, null);

        object? postState = await page.Load(preState);

        NameMirrorServices.Current.SynchronizationContext.Send((_) =>
        {
            page.PostLoad(postState);

            ContextData.IsBusy = false;
            UpdateNavigation();
        }, null);
    }

    private void UpdateNavigation()
    {
        CurrentPage.Update();
        CancelNavigationCommand.NotifyCanExecuteChanged();
        ReverseNavigationCommand.NotifyCanExecuteChanged();
        ForwardNavigationCommand.NotifyCanExecuteChanged();
    }

    // Navigation: Close (External)

    private bool CanExecuteClose()
        => !ContextData.IsBusy
        && CurrentPage.CanClose();

    private bool ExecuteClose()
    {
        // DO NOT Request view closure
        // Since the origin is from the view itself
        // And the cyclic call would result in stack overflow.
        return !ContextData.IsBusy
            && CurrentPage.Close();
    }

    // Navigation: Cancel
    // (In future, remove !IsBusy and change this to use cancel token instead.)
    // (Making everything async might be needed.)

    private bool CanExecuteCancel()
        => !ContextData.IsBusy
        && CurrentPage.CanCancel();

    private void ExecuteCancel()
    {
        if (!ContextData.IsBusy &&
            CurrentPage.Cancel())
            CloseView();
    }

    // Navigation: Reverse

    private bool CanExecuteReverse()
        => !ContextData.IsBusy
        && CurrentPage.CanReverse()
        && (_history.Count > 0 || CurrentPageId != WizardPageId.Start);

    private void ExecuteReverse()
    {
        if (CurrentPage.Reverse() is not WizardPageId previous)
        {
            if (_history.Count == 0)
                previous = WizardPageId.Start;
            else
                previous = _history.Pop();
        }

        SetPage(previous);
    }

    // Navigation: Forward

    private bool CanExecuteProgress()
        => !ContextData.IsBusy
        && CurrentPage.CanProgress();

    private void ExecuteProgress()
    {
        if (CurrentPage.Progress() is not WizardPageId next)
        {
            CloseView();
        }
        else
        {
            if (CurrentPageId != WizardPageId.Start)
                _history.Push(CurrentPageId);
            SetPage(next);
        }
    }
}