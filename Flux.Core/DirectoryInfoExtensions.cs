namespace Flux.Core;

public static class DirectoryInfoExtensions
{
    extension(string directoryPath)
    {
        public DirectoryInfo ToDirectory() => new DirectoryInfo(directoryPath);
    }
    extension(DirectoryInfo directory)
    {
        public FileInfo ToFile(string fileName) => new FileInfo(Path.Join(directory.FullName, fileName));
        public DirectoryInfo Join(string path) => new DirectoryInfo(Path.Join(directory.FullName, path));
    }
}