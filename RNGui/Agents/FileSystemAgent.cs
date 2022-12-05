using CSX.DotNet6.Y2022.ReferencedNaming.Agents;
using Microsoft.Win32;
using System;
using System.IO;

namespace CSX.Wpf.Y2022.RNGui.Agents
{
    public sealed class FileSystemAgent : IFileSystemAgent
    {
        // add wrapper methods for task/ref location save
        public string[]? GetFiles(string message, string initialDirectory = "", int filterIndex = 0)
        {
            // Prepare
            OpenFileDialog openFileDialog = new()
            {
                InitialDirectory = initialDirectory,
                Title = message,
                Filter = MyConfig.FileTypeFilters,
                FilterIndex = filterIndex,
                Multiselect = true,
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
            };

            //Query User
            if (openFileDialog.ShowDialog() != true)
            {
                return null;
            }

            // Deliver data
            return openFileDialog.FileNames;
        }
        public bool FileExists(string path)
            => File.Exists(path);
        public void MoveFile(string source, string destination)
            => File.Move(source, destination);

        // Path
        public string GetFullPath(string path)
            => Path.GetFullPath(path);
        public string GetFilename(string path)
            => Path.GetFileName(path) ?? string.Empty;
        public string GetDirectory(string path)
            => Path.GetDirectoryName(path) ?? string.Empty;
        public string GetFilenameWithoutExtension(string path)
            => Path.GetFileNameWithoutExtension(path) ?? string.Empty;
        public string GetExtension(string path)
            => Path.GetExtension(path) ?? string.Empty;
        public string Combine(string path1, string path2)
            => Path.Combine(path1, path2); // idk how to forward a full param-array without re-encapsulation, yet
    }
}