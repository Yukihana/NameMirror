using NameMirror.ViewContexts.WizardViewContext;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Windows.Data;

namespace NMGui.Support.XAML.Converters;

internal sealed partial class WizardStandardPageOrder : IValueConverter
{
    private static readonly ConcurrentDictionary<WizardPageId, string> _headers = [];

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Ensure input type
        if (value is not WizardPageId id)
            return null;

        // Handle page translation
        int index = id.ToRecommendedPageIndex();

        return index;
        /*
        // If above line fails, perhaps use this.
        if (targetType != typeof(int))
            return index;
        else if (targetType == typeof(string))
            return index.ToString();
        */
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}