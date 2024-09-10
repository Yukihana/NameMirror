using NameMirror.ViewContexts.WizardViewContext;
using System.Windows;

namespace NMGui.Views.Windows;

/// <summary>
/// Interaction logic for WizardWindow.xaml
/// </summary>
public partial class WizardWindow : Window
{
    private readonly WizardContextLogic _logic = new();

    public WizardWindow()
    {
        InitializeComponent();

        DataContext = _logic;
    }
}