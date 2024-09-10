using NameMirror.ViewContexts.WizardViewContext;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NMGui.Support.XAML.Converters;

internal sealed partial class WizardStepTitle : IValueConverter
{
    private static readonly ConcurrentDictionary<WizardPageId, string> _headers = [];

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Ensure types
        if (targetType != typeof(string) || value is not WizardPageId id)
            return null;

        // Attempt to retrieve cached value
        if (_headers.TryGetValue(id, out string cached))
            return cached;

        // Attempt to retrieve via resource key
        if (Application.Current.TryFindResource($"{id}WizardStepTitle") is not string found)
            return null;

        // Add to cache and return
        _headers.TryAdd(id, found);
        return found;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}