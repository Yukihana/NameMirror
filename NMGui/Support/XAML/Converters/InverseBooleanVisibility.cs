using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NMGui.Support.XAML.Converters;

internal class InverseBooleanVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool visiBool)
            throw new InvalidCastException($"Expected boolean. Received {(value?.GetType().Name ?? "null")} instead.");
        return visiBool ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Visibility visibility)
            throw new InvalidCastException($"Expected a visibility type. Received {(value?.GetType().Name ?? "null")} instead.");
        return visibility != Visibility.Visible;
    }
}