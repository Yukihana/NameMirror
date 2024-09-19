using NameMirror.Types;

namespace NameMirror.ServiceInterfaces;

public interface IFileInputService
{
    string[]? AddFiles(FileInputReason title, bool fromFolder = false);
}