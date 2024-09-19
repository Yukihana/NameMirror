namespace NameMirror.ViewContexts.Shared;

public readonly struct MoveTask(string _oldPath, string _newPath)
{
    public readonly string OldPath { get; } = _oldPath;
    public readonly string NewPath { get; } = _newPath;
}