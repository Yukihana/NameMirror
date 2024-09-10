using System;
using System.Threading;

namespace NameMirror;

public sealed partial class ServiceIndex
{
    public ServiceIndex()
    { }

    public ServiceIndex(SynchronizationContext synchronizationContext)
        => SynchronizationContext = synchronizationContext;

    // Singleton

    private static ServiceIndex? _current = null;

    public static ServiceIndex Current
        => _current ?? throw new InvalidOperationException($"A current {nameof(ServiceIndex)} has not been set.");

    // Singleton handlers

    public static ServiceIndex CreateDefault(SynchronizationContext synchronizationContext)
    {
        ServiceIndex result = new(synchronizationContext);
        MakeCurrent(result);
        return result;
    }

    public static void MakeCurrent(ServiceIndex serviceIndex)
    {
        if (_current is null)
            _current = serviceIndex;
        else
            throw new InvalidOperationException($"The current {nameof(ServiceIndex)} cannot be modified for security reasons.");
    }

    // Synchronization

    public SynchronizationContext SynchronizationContext { get; private set; } = SynchronizationContext.Current;
}