using CSX.DotNet.Shared.FileSystem.Filters;
using CSX.DotNet.Shared.FileSystem.Filters.WindowsDialogFormatting;
using System;
using System.IO;

namespace NMGui.Config;

public static class NMGuiConfig
{
    // AppData

    public const string ApplicationDataDirectoryName = "CSX_NameMirror";

    // Logs

    public const string LogsPrefix = "NameMirror";
    public static string LogsPath { get; }

    // Dialogs

    public static string LastTargetsPath { get; set; } = string.Empty;
    public static string LastRefsPath { get; set; } = string.Empty;
    public static FileDialogFilterConfiguration FileDialogFilterConfiguration { get; }
    public static string FileDialogFilterString { get; }

    // Lifecycle

    static NMGuiConfig()
    {
        LogsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            ApplicationDataDirectoryName,
            "Logs");

        FileDialogFilterConfiguration = DialogConfiguration.GetFileDialogFilterConfiguration();
        FileDialogFilterString = FileDialogFilterConfiguration.ToDialogFilterSyntax();
    }

    /*
    public static string ConfigPath { get; set; }
    public static string ConfigFileName { get; set; }
    */
}