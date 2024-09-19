using System.Linq;

namespace CSX.DotNet.Shared.FileSystem.Filters.WindowsDialogFormatting;

public static partial class FormattingExtensions
{
    public static string GetPatternsFilterSyntax(this FileDialogFilter filter)
        => string.Join(";", filter.Select(x => x.ToUpperInvariant()));

    public static string GetPatternsForLabel(
        this FileDialogFilter filter,
        string labelPatternPrefix,
        string labelPatternSeparator,
        string labelPatternSuffix)
        => $"{labelPatternPrefix}{string.Join(labelPatternSeparator, filter)}{labelPatternSuffix}";

    public static string ToDialogFilterSyntax(
        this FileDialogFilter filter,
        string labelPatternPrefix,
        string labelPatternSeparator,
        string labelPatternSuffix)
    {
        string patternsInLabel = filter.GetPatternsForLabel(
            labelPatternPrefix: labelPatternPrefix,
            labelPatternSeparator: labelPatternSeparator,
            labelPatternSuffix: labelPatternSuffix);
        return $"{filter.Title}{patternsInLabel}|{filter.GetPatternsFilterSyntax()}";
    }

    public static string ToDialogFilterSyntax(
        this FileDialogFilter filter,
        bool showPatternsInLabel = false)
    {
        string patternsInLabel = showPatternsInLabel
            ? filter.GetPatternsForLabel(
                labelPatternPrefix: " (",
                labelPatternSeparator: ", ",
                labelPatternSuffix: ")")
            : string.Empty;

        return $"{filter.Title}{patternsInLabel}|{filter.GetPatternsFilterSyntax()}";
    }

    public static string ToDialogFilterSyntax(
        this FileDialogFilterConfiguration config,
        string labelPatternPrefix,
        string labelPatternSeparator,
        string labelPatternSuffix)
    {
        var converted = config.Select(x => x.ToDialogFilterSyntax(
            labelPatternPrefix: labelPatternPrefix,
            labelPatternSeparator: labelPatternSeparator,
            labelPatternSuffix: labelPatternSuffix));
        return string.Join("|", converted);
    }

    public static string ToDialogFilterSyntax(
        this FileDialogFilterConfiguration config,
        bool showPatternsInLabel = true)
    {
        return string.Join("|", config.Select(x => x.ToDialogFilterSyntax(showPatternsInLabel)));
    }
}