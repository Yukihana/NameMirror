using NMGui.WInterop;
using System.Collections;
using System.Collections.Generic;

namespace NMGui.Support.Sorting;

/// <summary>
/// Non-Generic version
/// I'm not sure why it is called 'Sorter', as well as why it's handling the non generic cases.
/// </summary>
internal class NaturalSorter : IComparer<object>, IComparer
{
    public int Compare(object? x, object? y)
    {
        return
            SafeNativeMethods.StrCmpLogicalW(
                x as string ?? string.Empty,
                y as string ?? string.Empty);
        // if implementing direction, use:
        // *(SortDirection == ListSortDirection.Ascending ? 1 : -1)
    }
}