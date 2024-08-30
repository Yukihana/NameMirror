namespace ReferencedNaming.Agents;

public interface IHandler
{
    IFileSystemAgent FileSystemAgent { get; }
    IPromptAgent PromptAgent { get; }
}