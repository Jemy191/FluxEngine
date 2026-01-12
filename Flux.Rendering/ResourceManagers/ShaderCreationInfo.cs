namespace Flux.Rendering.ResourceManagers;

public readonly struct ShaderCreationInfo
{
    public readonly FileInfo VertexFile;
    public readonly FileInfo FragmentFile;
    public ShaderCreationInfo(FileInfo vertexFile, FileInfo fragmentFile)
    {
        VertexFile = vertexFile;
        FragmentFile = fragmentFile;
    }
}