using CSX.DotNet.Shared.Logging;
using CSX.DotNet.Shared.Threading;
using NameMirror.ViewContexts.Shared;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NameMirror.ViewContexts.WizardViewContext.WizardPages;

public partial class PageRename(IWizardData data) : IWizardPage
{
    public WizardPageId PageId => WizardPageId.Rename;

    public IWizardData Data { get; } = data;

    // Load

    public object? PreLoad()
    {
        Data.ProgressMode = WizardProgressMode.Close;
        Data.IsProgressIndeterminate = false;
        Data.ProgressValue = 0;
        Data.ProgressMaximum = 1;

        // Prepare the move roster
        return Data.RenameTasks.ToArray();
    }

    public async Task<object?> Load(object? state)
    {
        if (state is not RenameTask[] moveRoster)
            throw new ArgumentException("Bad input. Possible internal error.", nameof(state));
        // return the log string on rename complete.
        return await Rename(moveRoster); ;
    }

    public void PostLoad(object? state)
    {
        if (state is string log)
            Data.Log = log;
        _canProgress = true;

        Data.IsProgressIndeterminate = true;
    }

    // Update

    public void Update()
    {
    }

    // Close

    public bool CanClose() => true;

    public bool Close() => true;

    // Cancel

    public bool CanCancel() => false;

    public bool Cancel() => false;

    // Reverse

    public bool CanReverse() => false;

    public WizardPageId? Reverse() => null;

    // Progress

    public bool CanProgress() => _canProgress;

    public WizardPageId? Progress()
    {
        return null;
    }

    // Page specific

    private bool _canProgress = false;

    private const double UPDATE_INTERVAL_MILLISECONDS = 100;

    private readonly SynchronizationContext _syncContext = NameMirrorServices.Current.SynchronizationContext;
    private readonly ILoggingService _loggingService = NameMirrorServices.Current.LogService;

    private async Task<string> Rename(RenameTask[] moveRoster)
    {
        ConcurrentQueue<string> logs = [];
        logs.Enqueue("Starting rename operations... ");

        try
        {
            int progressMax = moveRoster.Length;
            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < progressMax; i++)
            {
                string oldName = moveRoster[i].Target.FileName;
                string newName = moveRoster[i].NewTargetName();
                string oldPath = moveRoster[i].Target.FullPath;
                string newPath = moveRoster[i].NewTargetPath();

                string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                try
                {
                    File.Move(oldPath, newPath);
                    // Local log
                    logs.Enqueue($"{timeStamp} - {newName}");
                    // Main logs
                    _loggingService.Log("information", $"Successfully moved {oldName} to {newName}.", this);
                }
                catch (Exception ex)
                {
                    logs.Enqueue($"{timeStamp} - FAIL: {oldName} => {newName}. Source:{ex.Source}. Description: {ex.Message}");
                    _loggingService.Log("error", $"Failed to move: {oldPath} => {newPath}", this, ex);
                }

                // Do update on interval
                if (stopwatch.ElapsedMilliseconds >= UPDATE_INTERVAL_MILLISECONDS)
                {
                    stopwatch.Restart();
                    await _syncContext.SendAsync(state =>
                    {
                        if (state is (double value, double maximum))
                            UpdateProgress(value, maximum);
                    }, (i, progressMax));
                }
            }

            logs.Enqueue("Rename operations completed.");
            logs.Enqueue("The events have also been added to the editor log.");
            logs.Enqueue("The wizard can now be closed safely.");
        }
        catch (Exception ex)
        {
            logs.Enqueue($"Error in Rename: {ex.Source}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
        }

        return string.Join(Environment.NewLine, logs);
    }

    private void UpdateProgress(double value, double max)
    {
        NameMirrorServices.Current.SynchronizationContext.Send(state =>
        {
            if (state is not (double progressValue, double progressMax))
                throw new ArgumentException();
            Data.ProgressValue = progressValue;
            Data.ProgressMaximum = progressMax;
        }, (value, max));
    }
}