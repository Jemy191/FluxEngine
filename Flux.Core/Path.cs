namespace Flux.Core;

public readonly record struct Path
{
    static bool IsWindows => System.IO.Path.DirectorySeparatorChar == '\\';
    readonly string path;

    public Path(string path) => this.path = IsWindows ? path.Replace('/', '\\') : path.Replace('\\', '/');

    public static implicit operator string(Path path) => path.path;
    public static implicit operator Path(string path) => new Path(path);
    
    public static Path operator /(Path left, Path right) => System.IO.Path.Combine(left, right);
}
