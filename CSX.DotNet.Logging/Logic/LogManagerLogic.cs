using CSX.DotNet.Logging.Commands;
using CSX.DotNet.Logging.Support;
using CSX.DotNet.Logging.Types;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace CSX.DotNet.Logging.Logic;

public class LogManagerLogic : INotifyPropertyChanged
{
    // Declarations

    #region Data : Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion Data : Events

    #region Data : Capacity

    private static readonly int[] LogCaps = new int[] { 0, 10, 50, 100, 200, 500, 1000, 2000, 5000, 9999 };
    private static readonly int DefaultStepIndex = 3;

    #endregion Data : Capacity

    // Objects
    private readonly Logger Logger;

    public string Directory;
    public Timer Timer;
    public LogEntry? lastElement = null;

    // Properties

    #region Property : LogEntries

    private readonly ObservableCollection<LogEntry> logEntries = new();
    public ObservableCollection<LogEntry> LogEntries => logEntries;

    #endregion Property : LogEntries

    #region Property : StepIndex

    // Property : StepIndex
    private double _stepIndex = -1;

    public double StepIndex
    {
        get
        {
            if (_stepIndex == -1)
            {
                _stepIndex = DefaultStepIndex;
            }
            return _stepIndex;
        }
        set
        {
            _stepIndex = (value >= 0 && value < LogCaps.Length) ? value : DefaultStepIndex;
            PropertyChanged?.Invoke(this, new(nameof(StepIndex)));
            UpdateLogCapacity();
            PropertyChanged?.Invoke(this, new(nameof(LogCapacity)));
        }
    }

    #endregion Property : StepIndex

    #region Property : StepIndex (Additional)

    public static double StepIndexMinimum => 0;
    public static double StepIndexMaximum => LogCaps.Length - 1;

    #endregion Property : StepIndex (Additional)

    #region Property : LogCapacity

    private int _logCapacity = -1;

    public int LogCapacity
    {
        get
        {
            if (_logCapacity < 0)
                UpdateLogCapacity();
            return _logCapacity;
        }
    }

    private void UpdateLogCapacity()
    {
        int stepping = (int)_stepIndex;
        _logCapacity = (stepping >= 0 && stepping < LogCaps.Length)
            ? LogCaps[stepping]
            : LogCaps[DefaultStepIndex];
    }

    #endregion Property : LogCapacity

    // Commands

    #region CycleLog

    private readonly ActionCommand _cycleLogCommand;
    public ActionCommand CycleLogCommand => _cycleLogCommand;

    public void CycleLog(object? parameter = null)
        => Task.Run(CycleLogAsync);

    #endregion CycleLog

    #region OpenDirectory

    private readonly ActionCommand _showInExplorerCommand;
    public ActionCommand ShowInExplorerCommand => _showInExplorerCommand;

    public static void ShowInExplorer(object? parameter = null)
    {
        Process.Start(new ProcessStartInfo("file:///c:\\") { UseShellExecute = true });
    }

    #endregion OpenDirectory

    #region DeleteOldLogs

    private readonly ActionCommand _deleteOldLogsCommand;
    public ActionCommand DeleteOldLogsCommand => _deleteOldLogsCommand;

    public bool SaveToDisk { get; private set; }

    private void DeleteOldLogs(object? parameter = null)
        => Task.Run(DeleteOldLogsAsync);

    private async void DeleteOldLogsAsync()
    {
        (int, int)? a = await Task.Run(() => Logger.DeleteOldLogs(Directory));
        lock (LogEntries)
        {
            if (a == null)
                LogEntries.Add(new("Failed to delete old logs."));
            else
                LogEntries.Add(new($"Deleted {a?.Item1} out of {a?.Item2}"));
        }
    }

    #endregion DeleteOldLogs

    // Lifetime

    #region Ctor, Dtor

    public LogManagerLogic() : this(string.Empty, "CsxLog")
    {
    }

    public LogManagerLogic(string directory, string prefix)
    {
        // Commands
        _cycleLogCommand = new(CycleLog);
        _showInExplorerCommand = new(ShowInExplorer);
        _deleteOldLogsCommand = new(DeleteOldLogs);

        // IO operation init
        Directory = directory;
        Logger = new(directory, prefix);

        // TaskDelay
        Timer = new(1000);
        Timer.AutoReset = true;
        Timer.Elapsed += Flush;
    }

    #endregion Ctor, Dtor

    // IO

    #region IO : Flush / Write

    private bool IsWriteBusy = false;

    private void Flush(object? sender, ElapsedEventArgs e)
    {
        if (IsWriteBusy)
            return;

        if (lastElement == LogEntries[^1])
            Task.Run(FlushAsync);
    }

    private void Flush() => Task.Run(FlushAsync);

    private async Task FlushAsync()
    {
        IsWriteBusy = true;

        // Prepare
        List<LogEntry> buffer = new();
        lock (LogEntries)
        {
            buffer.AddRange(LogEntries.Where(x => !x.Saved));
        }

        // Flush
        bool success = await Logger.WriteAsync(buffer);

        lock (LogEntries)
        {
            // Log flush report
            if (!success)
                LogEntries.Add(new("Failed to write log", LogLevel.Error));

            // Purge
            if (LogEntries.Count > LogCapacity)
            {
                int n = LogEntries.Count - LogCapacity;
                for (int i = 0; i < n; i++)
                {
                    if (LogEntries[i].Saved || !SaveToDisk)
                        LogEntries.RemoveAt(i);
                }
            }
        }

        IsWriteBusy = false;
    }

    #endregion IO : Flush / Write

    #region IO : Start / Cycle

    private async Task StartAsync()
    {
        await Task.Run(Logger.CycleLog);

        lock (LogEntries)
        {
            LogEntries.Add(new("Starting log session"));
        }
    }

    private async Task CycleLogAsync()
    {
        // Flush
        await FlushAsync();

        // Clean
        lock (LogEntries)
        {
            LogEntries.Clear();
        }

        // Start new session
        await StartAsync();
    }

    #endregion IO : Start / Cycle

    // Public Interface
    public void EnterLog(string message, string level, object? sender)
        => LogEntries.Add(new(message, level.Sanitize()));
}