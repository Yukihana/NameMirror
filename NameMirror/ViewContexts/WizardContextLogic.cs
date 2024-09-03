using CommunityToolkit.Mvvm.ComponentModel;

namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextLogic : ObservableObject
{
    [ObservableProperty]
    private int _pageIndex = 0;
}