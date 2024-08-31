namespace NameMirror.Agents;

public interface IHandler
{
    IFileSystemAgent FileSystemAgent { get; }
    IPromptAgent PromptAgent { get; }
}