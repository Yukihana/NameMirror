using CSX.DotNet.Logging.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CSX.DotNet.Logging;

internal class Logger
{
    private readonly string Location;
    private readonly string SessionTime;
    private readonly string Prefix;
    private string TargetLog = string.Empty;
    private ulong SubSessionIndex = 0;

    internal Logger(string directory, string prefix)
    {
        Location = directory;
        Prefix = prefix;
        SessionTime = DateTime.Now.ToString(format: "yyyyMMdd_HHmmss");
        TargetLog = GetNewLogName();
    }

    public void CycleLog()
       => TargetLog = GetNewLogName();

    private string GetNewLogName()
    {
        string path;
        do
        {
            path = Path.Combine(
                Location,
                $"{Prefix}_{SessionTime}_{SubSessionIndex++}.{AssemblyConfig.LogExtension}"
                );
        }
        while (File.Exists(path));

        return path;
    }

    // Async operations
    internal async Task<bool> WriteAsync(IEnumerable<LogEntry> logEntries)
    {
        try
        {
            // if file does not exist, create
            using StreamWriter sw = new(TargetLog, true, new UTF8Encoding());

            // write unsaved and update unsaved status
            foreach (LogEntry logEntry in logEntries)
            {
                string logstring = string.Empty;
                lock (logEntry)
                {
                    logstring = SynthesizeLogString(logEntry);
                }
                await sw.WriteLineAsync(logstring);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private string SynthesizeLogString(LogEntry logEntry)
    {
        throw new NotImplementedException();
    }

    internal static (int, int)? DeleteOldLogs(string directory)
    {
        int successes = 0;
        string[] list;

        try { list = Directory.GetFiles(directory, $"*.{AssemblyConfig.LogExtension}"); }
        catch (Exception) { return null; }

        foreach (string file in list)
        {
            try
            {
                File.Delete(file);
                successes++;
            }
            catch (Exception) { }
        }

        return (successes, list.Length);
    }

    /*
     * public async Task<bool> UpdateLog()
     * {
     *
     * }
     */
}