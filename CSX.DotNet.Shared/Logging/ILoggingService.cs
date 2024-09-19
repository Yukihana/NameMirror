using System;

namespace CSX.DotNet.Shared.Logging;

public interface ILoggingService
{
    void Log(string level, string message, object? sender = null, Exception? ex = null, params object[] attachments);
}