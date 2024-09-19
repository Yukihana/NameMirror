using NameMirror.ViewContexts.WizardViewContext;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NMGui.Support.XAML.Converters;

internal class WizardPageStringResource : IValueConverter
{
    private static readonly ConcurrentDictionary<string, string> _resources = [];

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Ensure types
        if (targetType != typeof(string) ||
            value is not WizardPageId id)
            return null;

        // Determine resource key
        string resourceKey
            = parameter is string suffix
            ? $"{id}{suffix}"
            : id.ToString();

        // Attempt to retrieve from cache
        if (_resources.TryGetValue(resourceKey, out string cached))
            return cached;

        // Attempt to retrieve from application resources
        if (Application.Current.TryFindResource(resourceKey) is not string found)
            return DependencyProperty.UnsetValue;

        // Add to cache and return
        _resources.TryAdd(resourceKey, found);
        return found;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}