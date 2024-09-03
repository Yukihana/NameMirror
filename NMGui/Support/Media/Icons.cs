using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NMGui.Support.Media;

public static partial class Icons
{
    // Gets the image with the provided resolution from icon, or null
    public static BitmapSource? GetImage(string resourcePath, int width, int height)
    {
        Uri iconUri = new(resourcePath, UriKind.RelativeOrAbsolute);
        Stream stream = Application.GetResourceStream(iconUri).Stream;
        BitmapDecoder decoder = BitmapDecoder.Create(
            stream,
            BitmapCreateOptions.PreservePixelFormat,
            BitmapCacheOption.None);
        BitmapFrame? frame = decoder.Frames.FirstOrDefault(x => x.PixelWidth == width && x.PixelHeight == height);
        return frame;
    }
}