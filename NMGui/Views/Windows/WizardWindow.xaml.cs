using NameMirror.ViewContexts.WizardViewContext;
using System.Windows;
using System.Windows.Controls;

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
        if (!_logic.IsClosingAllowed || !_logic.OnClosing())
            e.Cancel = true;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textbox)
            textbox.ScrollToEnd();
    }
}