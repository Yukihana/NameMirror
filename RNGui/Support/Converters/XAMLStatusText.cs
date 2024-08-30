using System;
using System.Globalization;
using System.Windows.Data;

namespace CSX.Wpf.Y2022.RNGui.Support.Converters;

internal class XAMLStatusText : IValueConverter
{
    private const string StatusChars = " ●✔✖";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => StatusChars[Math.Clamp((byte)value, (byte)0, (byte)3)].ToString();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}