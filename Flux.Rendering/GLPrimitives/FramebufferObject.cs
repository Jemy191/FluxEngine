using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives;

public readonly struct FramebufferObject : IDisposable
{
    const FramebufferTarget FramebufferTarget = Silk.NET.OpenGL.FramebufferTarget.Framebuffer;
    readonly GL gl;

    readonly uint handle;

    public FramebufferObject(GL gl, FramebufferAttachmentSetting[] attachments)
    {
        this.gl = gl;

        handle = gl.GenFramebuffer();

        Bind();

        foreach (var attachment in attachments)
        {
            gl.FramebufferTexture2D(FramebufferTarget, attachment.Attachment, TextureTarget.Texture2D, attachment.TextureHandle, 0);
        }

        var drawBuffers = attachments
            .Select(a => a.Attachment)
            .Where(a => a
                is >= FramebufferAttachment.ColorAttachment0
                and <= FramebufferAttachment.ColorAttachment31)
            .Select(a => (DrawBufferMode)a)
            .ToArray();

        if(drawBuffers.Length != 0 && drawBuffers is not [DrawBufferMode.ColorAttachment0])
            gl.DrawBuffers(drawBuffers);

        if(gl.CheckFramebufferStatus(FramebufferTarget) is not GLEnum.FramebufferComplete)
            throw new RendererException("Framebuffer is incomplete");
    }

    public void Bind() => gl.BindFramebuffer(FramebufferTarget, handle);

    public void Dispose()
    {
        gl.DeleteFramebuffer(handle);
    }
}