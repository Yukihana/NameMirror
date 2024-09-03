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

        DataContext = _logic;
    }

    private void WizTab_Click(object sender, RoutedEventArgs e)
        => WizPager.SelectedIndex = WizTabPanel.Children.IndexOf(sender as RadioButton);
}