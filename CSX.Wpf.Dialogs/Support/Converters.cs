using CSX.Wpf.Dialogs.Types;
using System;
using System.Windows;

namespace CSX.Wpf.Dialogs.Support
{
    internal static class Converters
    {
        public static AlertLevel ToAlertLevel(this string level)
            => level.Trim().ToLowerInvariant() switch
            {
                "query" => AlertLevel.Query,
                "warning" => AlertLevel.Warning,
                "error" => AlertLevel.Error,
                _ => AlertLevel.Information,
            };
        public static MessageBoxImage ToMessageBoxImage(this string level)
            => level.Trim().ToLowerInvariant() switch
            {
                "query" => MessageBoxImage.Question,
                "warning" => MessageBoxImage.Warning,
                "error" => MessageBoxImage.Error,
                _ => MessageBoxImage.Information,
            };
        public static int ToPositiveIndex(this int n, int total)
            => Math.Clamp(n < 0 ? total + n : n, 0, total - 1);
        public static int ToNegativeIndex(this int n, int total)
            => Math.Clamp(Math.Abs(n) - total, -1, 0 - total);
    }
}