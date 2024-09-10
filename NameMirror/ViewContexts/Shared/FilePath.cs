using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;

namespace NameMirror.ViewContexts.Shared;

public partial class FilePath : ObservableObject
{
    // Primary properties

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullPath))]
    private string _parentDirectory = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FileName))]
    [NotifyPropertyChangedFor(nameof(FullPath))]
    private string _fileNameWithoutExtension = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullPath))]
    private string _extension = string.Empty;

    // Extended properties

    public string FileName =>
        $"{FileNameWithoutExtension}{Extension}";

    public string FullPath =>
        Path.Combine(ParentDirectory, FileName);

    // Overrides

    public override string ToString() => FullPath;

    // Factory

    public static FilePath Create(string fullPath) => new()
    {
        ParentDirectory = Path.GetDirectoryName(fullPath) ?? string.Empty,
        FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath) ?? string.Empty,
        Extension = Path.GetExtension(fullPath) ?? string.Empty
    };
}