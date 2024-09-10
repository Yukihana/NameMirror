using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSX.DotNet.Shared.Threading;

public static partial class SynchronizationExtensions
{
    public static Task SendAsync(
        this SynchronizationContext context,
        SendOrPostCallback d,
        object? stateObject)
    {
        TaskCompletionSource<bool> tcs = new();
        context.Post((state) =>
        {
            try
            {
                d(state);
                tcs.SetResult(true);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }, stateObject);
        return tcs.Task;
    }
}