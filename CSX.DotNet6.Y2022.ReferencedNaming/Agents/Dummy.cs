using System;
using System.Diagnostics;

namespace CSX.DotNet6.Y2022.ReferencedNaming.Agents;

[DebuggerStepThrough]
internal class Dummy : IHandler, IFileSystemAgent, IPromptAgent
{
    public IFileSystemAgent FileSystemAgent => throw new NotImplementedException();
    public IPromptAgent PromptAgent => throw new NotImplementedException();
    public void Alert(string message, string title = "Alert", string level = "information") => throw new NotImplementedException();
    public string Combine(string path1, string path2) => throw new NotImplementedException();
    public bool FileExists(string path) => throw new NotImplementedException();
    public string GetDirectory(string path) => throw new NotImplementedException();
    public string GetExtension(string path) => throw new NotImplementedException();
    public string GetFilename(string path) => throw new NotImplementedException();
    public string GetFilenameWithoutExtension(string path) => throw new NotImplementedException();
    public string[]? GetFiles(string message, string initialDirectory = "", int filterIndex = 0) => throw new NotImplementedException();
    public string GetFullPath(string path) => throw new NotImplementedException();
    public bool Invalidate(string? result, string invalid = "Cancel") => throw new NotImplementedException();
    public void MoveFile(string source, string destination) => throw new NotImplementedException();
    public void PlaySound(string soundType) => throw new NotImplementedException();
    public string? Query(string message, string title = "Query", string buttons = "OK;Cancel", string level = "information", string defaultButton = "OK", string cancelButton = "Cancel", object? customImage = null) => throw new NotImplementedException();
    public bool Validate(string? result, string valid = "OK") => throw new NotImplementedException();
}