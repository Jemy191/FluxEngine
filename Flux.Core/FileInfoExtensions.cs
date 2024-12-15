namespace Flux.Core;

public static class FileInfoExtensions
{
    public static FileInfo ToFile(this string filePath) => new FileInfo(filePath);
}