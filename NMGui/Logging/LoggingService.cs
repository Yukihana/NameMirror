using CSX.DotNet.Logging.Logic;
using CSX.DotNet.Shared.Logging;
using NMGui.Config;
using System;

namespace NMGui.Logging;

public partial class LoggingService : ILoggingService
{
    // Context Logic

    public LogManagerLogic ContextLogic { get; } = new(NMGuiConfig.LogsPath, NMGuiConfig.LogsPrefix);

    // Log functions

    public void Log(string level, string message, object? sender = null, Exception? ex = null, params object[] attachments)
    {
        ContextLogic.EnterLog(message, level, sender);
    }
}