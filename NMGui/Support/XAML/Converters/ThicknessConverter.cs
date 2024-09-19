using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NMGui.Support.XAML.Converters;

internal partial class ThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Thickness thickness)
            return DependencyProperty.UnsetValue;

        if (parameter is null)
            return Average(thickness);
        if (parameter is not string resolutionType)
            return DependencyProperty.UnsetValue;

        return resolutionType.ToLowerInvariant() switch
        {
            "average" => Average(thickness),
            "horizontal" => (thickness.Left + thickness.Right) / 2,
            "vertical" => (thickness.Top + thickness.Bottom) / 2,
            "left" => thickness.Left,
            "top" => thickness.Top,
            "right" => thickness.Right,
            "bottom" => thickness.Bottom,
            _ => DependencyProperty.UnsetValue,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double d)
            return new Thickness(d);

        if (value is float f)
            return new Thickness(f);

        if (value is int i)
            return new Thickness(i);

        return DependencyProperty.UnsetValue;
    }

    // Assists

    public static double Average(Thickness thickness)
        => (thickness.Left + thickness.Top + thickness.Right + thickness.Bottom) / 4;
}