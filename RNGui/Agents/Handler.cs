using ReferencedNaming.Agents;

namespace CSX.Wpf.Y2022.RNGui.Agents;

internal class Handler : IHandler
{
    #region FileSystemAgent

    private readonly FileSystemAgent _fileSystemAgent;
    public IFileSystemAgent FileSystemAgent => _fileSystemAgent;

    #endregion FileSystemAgent

    #region PromptAgent

    private readonly PromptAgent _promptAgent;
    public IPromptAgent PromptAgent => _promptAgent;

    #endregion PromptAgent

    #region Ctor

    public Handler()
    {
        _fileSystemAgent = new();
        _promptAgent = new();
    }

    #endregion Ctor
}