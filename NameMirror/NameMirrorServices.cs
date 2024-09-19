using CSX.DotNet.Shared.Alerts;
using CSX.DotNet.Shared.FileSystem;
using CSX.DotNet.Shared.Logging;
using NameMirror.Agents;
using NameMirror.ServiceInterfaces;
using System;
using System.Threading;

namespace NameMirror;

public sealed partial class NameMirrorServices
{
    public NameMirrorServices(
        SynchronizationContext synchronizationContext,
        IFileInputService fileInputService,
        IAlertService alertService,
        IPromptAgent promptAgent,
        ILoggingService loggingService)
    {
        _synchronizationContext = synchronizationContext;
        _fileInputService = fileInputService;
        _alertService = alertService;
        _promptAgent = promptAgent;
        _loggingService = loggingService;
    }

    // Singleton

    private static NameMirrorServices? _current = null;

    public static NameMirrorServices Current
        => _current ?? throw new InvalidOperationException($"A current {nameof(NameMirrorServices)} has not been set.");

    // Singleton handlers

    public static void MakeCurrent(NameMirrorServices serviceIndex)
    {
        if (_current is null)
            _current = serviceIndex;
        else
            throw new InvalidOperationException($"The current {nameof(NameMirrorServices)} cannot be modified for security reasons.");
    }
}