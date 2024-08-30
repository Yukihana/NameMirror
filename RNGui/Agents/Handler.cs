using ReferencedNaming.Agents;

namespace RNGui.Agents;

internal class Handler : IHandler
{
    private readonly FileSystemAgent _fileSystemAgent;
    public IFileSystemAgent FileSystemAgent => _fileSystemAgent;

    private readonly PromptAgent _promptAgent;
    public IPromptAgent PromptAgent => _promptAgent;

    public Handler()
    {
        _fileSystemAgent = new();
        _promptAgent = new();
    }
}