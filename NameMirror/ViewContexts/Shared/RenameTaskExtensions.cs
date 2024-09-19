using System.IO;

namespace NameMirror.ViewContexts.Shared;

public static partial class RenameTaskExtensions
{
    public static string NewTargetName(this RenameTask renameTask)
        => $"{renameTask.Reference.FileNameWithoutExtension}{renameTask.Target.Extension}";

    public static string NewTargetPath(this RenameTask renameTask) => Path.Combine(
        renameTask.Target.ParentDirectory,
        renameTask.NewTargetName());
}