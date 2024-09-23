using NameMirror.Types;
using System.IO;

namespace NameMirror.ViewContexts.Shared;

public static partial class RenameTaskExtensions
{
    public static string NewTargetName(this RenameTask renameTask)
        => $"{renameTask.Reference.FileNameWithoutExtension}{renameTask.Target.Extension}";

    public static string NewTargetPath(this RenameTask renameTask) => Path.Combine(
        renameTask.Target.ParentDirectory,
        renameTask.NewTargetName());

    public static byte GetStatusIndex(this RNTask renameTask)
    {
        if (renameTask.SuccessStatus == false)
            return 3;
        else if (renameTask.SuccessStatus == true)
            return 2;
        else if (renameTask.Ready)
            return 1;
        else
            return 0;
    }

    public static bool GetReady(this RNTask renameTask)
        => !string.IsNullOrWhiteSpace(renameTask.TargetDirectory)
        && !string.IsNullOrEmpty(renameTask.ReferenceFileNameWithoutExtension);

    // Legacy

    public static byte GetStatusIndex(this RNTaskLegacy renameTask)
    {
        if (renameTask.SuccessStatus == false)
            return 3;
        else if (renameTask.SuccessStatus == true)
            return 2;
        else if (renameTask.Ready)
            return 1;
        else
            return 0;
    }
}