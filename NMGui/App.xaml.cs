using NameMirror;
using System.Threading;
using System.Windows;

namespace NMGui;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceIndex _logicService;

    public App()
    {
        _logicService = ServiceIndex.CreateDefault(
            synchronizationContext: SynchronizationContext.Current);
    }
}