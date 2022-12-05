namespace CSX.DotNet6.Y2022.ReferencedNaming.Agents;

public interface IHandler
{
    IFileSystemAgent FileSystemAgent { get; }
    IPromptAgent PromptAgent { get; }
}