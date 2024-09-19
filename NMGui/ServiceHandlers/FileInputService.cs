using CSX.DotNet.Shared.FileSystem.Filters.WindowsDialogFormatting;
using Microsoft.Win32;
using NameMirror.ServiceInterfaces;
using NameMirror.Types;
using NMGui.Config;
using NMGui.Support.Sorting;
using Ookii.Dialogs.Wpf;
using System;
using System.IO;
using System.Windows;

namespace NMGui.ServiceHandlers;

public partial class FileInputService : IFileInputService
{
    private static readonly string _filters = NMGuiConfig.FileDialogFilterConfiguration.ToDialogFilterSyntax();
    private readonly Application _app;

    // Lifecycle

    public FileInputService()
        => _app = Application.Current;

    // Interface :  IFileInputService (NameMirror)

    public string[]? AddFiles(FileInputReason reason, bool fromFolder = false)
    {
        // Determine title
        string? title = _app.TryFindResource($"AddFileDialogTitle{reason}") as string;

        // Determine initialDirectory (Targets/References/Other)
        (string? initialDirectory, int filterIndex) = TranslateReason(reason);

        // Finish
        return fromFolder
            ? AddFromFolder(title, initialDirectory)
            : AddFiles(title, initialDirectory, filterIndex);
    }

    // Interface :  IFileSystemService (DotNet.Shared)

    public string[]? AddFiles(
        string? title = null,
        string? initialDirectory = null,
        int filterIndex = 0)
    {
        // Prepare
        OpenFileDialog openFileDialog = new()
        {
            Filter = _filters,
            Multiselect = true,
            CheckFileExists = true,
            CheckPathExists = true,
            ValidateNames = true,
        };

        if (!string.IsNullOrWhiteSpace(title))
            openFileDialog.Title = title;
        if (!string.IsNullOrWhiteSpace(initialDirectory))
            openFileDialog.InitialDirectory = initialDirectory;
        if (filterIndex != 0)
            openFileDialog.FilterIndex = filterIndex;

        //Query User
        if (openFileDialog.ShowDialog() != true)
            return null;

        // Deliver data
        return openFileDialog.FileNames;
    }

    public string[]? AddFromFolder(
        string? title = null,
        string? initialDirectory = null)
    {
        VistaFolderBrowserDialog folderDialog = new()
        {
            UseDescriptionForTitle = true,
        };

        if (!string.IsNullOrWhiteSpace(title))
            folderDialog.Description = title;
        if (!string.IsNullOrWhiteSpace(initialDirectory))
            folderDialog.SelectedPath = initialDirectory;

        if (folderDialog.ShowDialog() != true)
            return null;

        try
        {
            var files = Directory.GetFiles(folderDialog.SelectedPath, "*", SearchOption.TopDirectoryOnly);
            Array.Sort(files, NaturalStringComparer.Default);
            return files;
        }
        catch { return null; }
    }

    // Translators

    private static (string? initialDirectory, int filterIndex) TranslateReason(FileInputReason reason)
    {
        if (reason
            is FileInputReason.AddTargets
            or FileInputReason.InsertTargets)
        {
            return (NMGuiConfig.LastTargetsPath, 4);
        }
        else if (reason
            is FileInputReason.AddReferences
            or FileInputReason.AppendReferences
            or FileInputReason.ReplaceReferencesAt
            or FileInputReason.ReplaceAllReferences)
        {
            return (NMGuiConfig.LastRefsPath, 2);
        }
        return (null, 0);
    }
}