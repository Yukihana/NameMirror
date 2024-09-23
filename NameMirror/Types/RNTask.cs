using CommunityToolkit.Mvvm.ComponentModel;
using NameMirror.ViewContexts.Shared;
using System;
using System.IO;

namespace NameMirror.Types;

public partial class RNTask : ObservableObject
{
    // Target

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TargetFullPath))]
    private string _targetDirectory = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TargetFileName))]
    [NotifyPropertyChangedFor(nameof(TargetFullPath))]
    private string _targetFileNameWithoutExtension = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TargetFileName))]
    [NotifyPropertyChangedFor(nameof(TargetFullPath))]
    private string _targetExtension = string.Empty;

    // Reference

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ReferenceFullPath))]
    private string _referenceDirectory = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ReferenceFileName))]
    [NotifyPropertyChangedFor(nameof(ReferenceFullPath))]
    private string _referenceFileNameWithoutExtension = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ReferenceFileName))]
    [NotifyPropertyChangedFor(nameof(ReferenceFullPath))]
    private string _referenceExtension = string.Empty;

    // Derived : Base

    public string TargetFileName
        => $"{TargetFileNameWithoutExtension}{TargetExtension}";

    public string TargetFullPath => Path.Combine(
        TargetDirectory,
        TargetFileName);

    public string ReferenceFileName
        => $"{ReferenceFileNameWithoutExtension}{ReferenceExtension}";

    public string ReferenceFullPath => Path.Combine(
        ReferenceDirectory,
        ReferenceFileName);

    // Derived : Secondary

    public string GeneratedFileName
        => $"{ReferenceFileNameWithoutExtension}{TargetExtension}";

    public string GeneratedFullPath => Path.Combine(
        TargetDirectory,
        GeneratedFileName);

    // Custom Property : Preview

    private string _previewFileName = string.Empty;

    /// <summary>
    /// Also contains extension.
    /// </summary>
    public string PreviewFileName
    {
        get
        {
        }
        set
        {
        }
    }

    // Derived

    public string PreviewFullPath
        => Path.Combine(TargetDirectory, PreviewFileName);

    [ObservableProperty]
    private bool _isSelected = false;

    [ObservableProperty]
    private Exception? _lastException = null;

    // [ObservableProperty]

    // Derived Properties

    public bool IsReady => this.Get

    public byte StatusIndex => this.GetStatusIndex();
}