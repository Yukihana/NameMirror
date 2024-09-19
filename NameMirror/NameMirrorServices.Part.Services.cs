using CSX.DotNet.Shared.Alerts;
using CSX.DotNet.Shared.Logging;
using NameMirror.Agents;
using NameMirror.ServiceInterfaces;
using System.Threading;

namespace NameMirror;

public sealed partial class NameMirrorServices
{
    // Backing fields

    private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
    private readonly IFileInputService _fileInputService;
    private readonly IAlertService _alertService;
    private readonly ILoggingService _loggingService;

    private readonly IPromptAgent _promptAgent;

    // Public Properties

    public SynchronizationContext SynchronizationContext => _synchronizationContext;
    public IFileInputService FileInputService => _fileInputService;
    public IAlertService AlertService => _alertService;
    public IPromptAgent PromptAgent => _promptAgent;
    public ILoggingService LogService => _loggingService;
}