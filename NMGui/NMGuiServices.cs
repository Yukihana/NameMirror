using System;
using System.Threading;

namespace NMGui;

internal partial class NMGuiServices
{
    public NMGuiServices(SynchronizationContext syncContext)
    {
        SynchronizationContext = syncContext;
    }

    // Singleton

    private static NMGuiServices? _current = null;

    public static NMGuiServices Current
        => _current ?? throw new InvalidOperationException($"A current {nameof(NMGuiServices)} has not been set.");

    public static void MakeCurrent(NMGuiServices serviceIndex)
    {
        if (_current is null)
            _current = serviceIndex;
        else
            throw new InvalidOperationException($"The current {nameof(NMGuiServices)} cannot be modified for security reasons.");
    }
}