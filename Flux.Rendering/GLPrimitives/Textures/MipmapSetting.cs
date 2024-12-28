using Dunet;

namespace Flux.Rendering.GLPrimitives.Textures;

[Union]
public partial record MipmapSetting
{
    partial record NoMipmap();
    partial record Mipmap(int TextureBaseLevel, int TextureMaxLevel);
}