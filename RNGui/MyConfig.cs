using System;
using System.IO;

namespace RNGui;

internal static class MyConfig
{
    /*
    public static string ConfigPath { get; set; }
    public static string ConfigFileName { get; set; }
    */

    // Hardcoded
    internal const string ApplicationPathTitle = "RNT";

    internal const string LogsPrefix = "ReferencedNamingTool";
    internal const string FileTypeFilters = "All files (*.*)|*.*|Image files (*.jpg, *.jpeg, *.bmp, *.png, *.gif)|*.JPG;*.JPEG;*.BMP;*.PNG;*.GIF|Document files (*.docx, *.doc, *.pdf, *.rtf)|*.DOCX;*.DOC;*.PDF;*.RTF";

    // Derived
    public static string LogsPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        ApplicationPathTitle,
        "Logs");

    // Muted
    public static string LastTasksPath { get; set; } = string.Empty;

    public static string LastRefsPath { get; set; } = string.Empty;
}