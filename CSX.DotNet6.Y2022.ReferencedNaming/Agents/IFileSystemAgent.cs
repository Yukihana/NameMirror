namespace CSX.DotNet6.Y2022.ReferencedNaming.Agents
{
    public interface IFileSystemAgent
    {
        // File
        public bool FileExists(string path);
        public void MoveFile(string source, string destination);

        // Path
        public string GetFullPath(string path);
        public string GetFilename(string path);
        public string GetDirectory(string path);
        public string GetFilenameWithoutExtension(string path);
        public string GetExtension(string path);
        public string Combine(string path1, string path2);

        // FrontEnd invoke, unnecessary, handled by View
        public string[]? GetFiles(string message, string initialDirectory = "", int filterIndex = 0);
    }
}