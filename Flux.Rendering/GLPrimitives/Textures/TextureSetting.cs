using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives.Textures
{
    public class TextureSetting
    {
        public required TextureWrapMode WrapModeS { get; init; }
        public required TextureWrapMode WrapModeT { get; init; }
        public required TextureMinFilter TextureMinFilter { get; init; }
        public required TextureMagFilter TextureMagFilter { get; init; }
        public required MipmapSetting Mipmap { get; init; }
    }
}
