using NMGui.Support;
using System.Windows;
using System.Windows.Controls;

namespace NMGui.Views.Windows;

/// <summary>
/// Interaction logic for WizardWindow.xaml
/// </summary>
public partial class WizardWindow : Window
{
    private readonly WizardData _data = new();
    internal WizardData Data => _data;

    public WizardWindow()
    {
        InitializeComponent();

        DataContext = Data;
    }

    private void WizTab_Click(object sender, RoutedEventArgs e)
        => WizPager.SelectedIndex = WizTabPanel.Children.IndexOf(sender as RadioButton);
}