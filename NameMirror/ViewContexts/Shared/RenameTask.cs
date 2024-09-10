using CommunityToolkit.Mvvm.ComponentModel;

namespace NameMirror.ViewContexts.Shared;

public partial class RenameTask : ObservableObject
{
    [ObservableProperty]
    private FilePath _target = new();

    [ObservableProperty]
    private FilePath _reference = new();
}