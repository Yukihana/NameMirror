using CommunityToolkit.Mvvm.ComponentModel;

namespace NameMirror.ViewContexts.Shared;

public partial class RenameTask : ObservableObject
{
    public RenameTask()
    {
    }

    public RenameTask(FilePath target, FilePath reference)
    {
        Target = target;
        Reference = reference;
    }

    [ObservableProperty]
    private FilePath _target = new();

    [ObservableProperty]
    private FilePath _reference = new();
}