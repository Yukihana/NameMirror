using NMGui.WInterop;
using System.Collections.Generic;

namespace NMGui.Support.Sorting;

/// <summary>
/// Used for Natural-Sorting using the generic IComparer.
/// </summary>
internal partial class NaturalStringComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        return
            SafeNativeMethods.StrCmpLogicalW(
                x ?? string.Empty,
                y ?? string.Empty);
        // if implementing direction, use:
        // *(SortDirection == ListSortDirection.Ascending ? 1 : -1)
    }

    // Singleton

    private static NaturalStringComparer? _default = null;
    public static NaturalStringComparer Default => _default ??= new();
}