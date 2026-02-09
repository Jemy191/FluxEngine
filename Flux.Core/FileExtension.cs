namespace Flux.Core;

public readonly record struct FileExtension
{
    public readonly string Extension;

    /// <summary> Will trim the leading dot from the file extension. </summary>
    public FileExtension(string extension) => Extension = extension.TrimStart('.');

    public override string ToString() => $".{Extension}";
}