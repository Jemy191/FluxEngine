namespace Flux.Core;

public static class DirectoryInfoExtensions
{
    public static DirectoryInfo ToDirectory(this string directoryPath) => new DirectoryInfo(directoryPath);
    public static FileInfo ToFile(this DirectoryInfo directory, string fileName) => new FileInfo(Path.Combine(directory.FullName, fileName));
    public static DirectoryInfo Combine(this DirectoryInfo directory, string path) => new DirectoryInfo(Path.Combine(directory.FullName, path));
}