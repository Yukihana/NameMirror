using CSX.DotNet.Shared.FileSystem.Filters;

namespace NMGui.Config;

public static partial class DialogConfiguration
{
    public static FileDialogFilterConfiguration GetFileDialogFilterConfiguration()
    {
        // All
        FileDialogFilter allFiles = ["*.*"];
        allFiles.Title = "All files";

        // Images
        FileDialogFilter images = ["*.jpg", "*.jpeg", "*.bmp", "*.png", "*.gif"];
        images.Title = "Images";

        // Documents
        FileDialogFilter documents = ["*.docx", "*.doc", "*.pdf", "*.rtf"];
        documents.Title = "Documents";

        // Group and serialize
        return [allFiles, images, documents];
    }
}