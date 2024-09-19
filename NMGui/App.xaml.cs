using NameMirror;
using NMGui.Agents;
using NMGui.Logging;
using NMGui.ServiceHandlers;
using System.Threading;
using System.Windows;

namespace NMGui;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public LoggingService LoggingService { get; } = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        _ = CreateNameMirrorServices(); // Do this after SyncContext has been initialized (Not possible in ctor)
        base.OnStartup(e);
    }

    private NameMirrorServices CreateNameMirrorServices()
    {
        NameMirrorServices services = new(
            synchronizationContext: SynchronizationContext.Current,
            fileInputService: new FileInputService(),
            alertService: new AlertService(),
            promptAgent: new PromptAgent(),
            loggingService: LoggingService);
        NameMirrorServices.MakeCurrent(services);
        return services;
    }
}