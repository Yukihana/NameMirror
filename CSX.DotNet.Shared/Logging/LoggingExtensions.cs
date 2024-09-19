using CSX.DotNet.Shared.Logging;
using System;

namespace NMGui.Logging;

public static partial class LoggingExtensions
{
    public static void LogError(
        this ILoggingService service,
        Exception? ex,
        string message,
        object? sender = null,
        params object[] attachments)
    {
        throw new NotImplementedException();
    }
}