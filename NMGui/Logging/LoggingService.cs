using CSX.DotNet.Logging.Logic;
using CSX.DotNet.Shared.Logging;
using NMGui.Config;
using System;
using System.Windows;

namespace NMGui.Logging;

public partial class LoggingService : ILoggingService
{
    // Context Logic

    public LogManagerLogic ContextLogic { get; } = new(NMGuiConfig.LogsPath, NMGuiConfig.LogsPrefix);

    // Log functions

    public void Log(string level, string message, object? sender = null, Exception? ex = null, params object[] attachments)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            ContextLogic.EnterLog(message, level, sender);
        }, null);
    }

    // Add a templated log system for language compatibility.
    // E.g. with event id
}