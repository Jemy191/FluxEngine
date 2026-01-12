using Flux.Rendering.GLPrimitives.Textures;

namespace Flux.Rendering.ResourceManagers;

public readonly struct TextureCreationInfo
{
    public readonly FileInfo File;
    public readonly TextureSetting Setting;
    public TextureCreationInfo(FileInfo file, TextureSetting setting)
    {
        this.File = file;
        this.Setting = setting;
    }
}