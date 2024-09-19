using CSX.DotNet.Shared.FileSystem.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace CSX.DotNet.Shared.FileSystem;

public interface IFileSystemService
{
    //

    string[]? AddFiles(
        string message,
        string initialDirectory,
        FileDialogFilterConfiguration filters,
        int filterIndex = 0);

    string[]? AddFromFolder(
        string message,
        string initialDirectory);

    string? AddFolder();

    // Activate In Shell

    void RunInShell();

    Task<object>? RunInShellAsync(CancellationToken cancelToken = default);

    // Show In Shell

    void ShowInFileManager(); // additionally, if it's a file, it selects that in the file manager.
}