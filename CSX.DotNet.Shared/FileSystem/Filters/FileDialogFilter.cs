using System.Collections.Generic;

namespace CSX.DotNet.Shared.FileSystem.Filters;

public partial class FileDialogFilter : List<string>
{
    public string Title { get; set; } = "FilterDefault";
}