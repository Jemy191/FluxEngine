namespace Flux.Core;

public static class Extensions
{
    public static FileInfo ToFile(this string filePath) => new FileInfo(filePath);
}