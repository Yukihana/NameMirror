using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace CSX.Wpf.Controls;

internal class WizTabControl : TabControl
{
    public DockPosition HeaderDock
    {
        get { return (DockPosition)GetValue(HeaderDockProperty); }
        set { SetValue(HeaderDockProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HeaderDockProperty =
        DependencyProperty.Register(nameof(HeaderDock), typeof(DockPosition), typeof(WizTabControl), new PropertyMetadata(0));
}