namespace Flux.Rendering.ResourceManagers;

public readonly struct ShaderCreationInfo
{
    public readonly FileInfo shaderFile;
    public ShaderCreationInfo(FileInfo shaderFile) => this.shaderFile = shaderFile;
}