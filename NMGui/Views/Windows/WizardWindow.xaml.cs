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

        _logic.RequestClose += (s, e) => Close();

        DataContext = _logic;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (!_logic.CanCloseView())
            e.Cancel = true;
    }
}