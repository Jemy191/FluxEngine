using Silk.NET.OpenGL;
using Texture = Flux.Rendering.GLPrimitives.Textures.Texture;

namespace Flux.Rendering.GLPrimitives;

public readonly struct FramebufferAttachmentSetting
{
    public readonly FramebufferAttachment Attachment;
    public readonly uint TextureHandle;
    public FramebufferAttachmentSetting(FramebufferAttachment attachment, Texture texture)
    {
        Attachment = attachment;
        TextureHandle = texture.Handle;
    }
}