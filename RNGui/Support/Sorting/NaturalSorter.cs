using RNGui.WInterop;
using System.Collections;
using System.Collections.Generic;

namespace RNGui.Support.Sorting;

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