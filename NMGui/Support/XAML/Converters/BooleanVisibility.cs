using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NMGui.Support.XAML.Converters;

internal class BooleanVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool visiBool)
            throw new InvalidCastException($"Expected boolean. Received {(value?.GetType().Name ?? "null")} instead.");

        // Visible : True
        if (visiBool)
            return Visibility.Visible;

        // String Parameter
        if (parameter is string stringConcealType)
        {
            if (stringConcealType.Equals("hidden", StringComparison.OrdinalIgnoreCase))
                return Visibility.Hidden;
            if (stringConcealType.Equals("collapsed", StringComparison.OrdinalIgnoreCase))
                return Visibility.Collapsed;
        }

        // Visibility Parameter
        if (parameter is Visibility visibilityConcealType && visibilityConcealType is not Visibility.Visible)
            return visibilityConcealType;

        // Hidden : Default
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Visibility visibility)
            throw new InvalidCastException($"Expected a visibility type. Received {(value?.GetType().Name ?? "null")} instead.");
        return visibility == Visibility.Visible;
    }
}