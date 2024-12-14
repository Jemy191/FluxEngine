using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct BufferObject<TDataType> : IDisposable
    where TDataType : unmanaged
{
    readonly uint handle;
    readonly BufferTargetARB bufferType;
    readonly GL gl;

    public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB bufferType)
    {
        this.gl = gl;
        this.bufferType = bufferType;

        handle = this.gl.GenBuffer();
        Bind();
        fixed (void* d = data)
        {
            var size = (nuint)(data.Length * sizeof(TDataType));
            gl.BufferData(bufferType, size, d, BufferUsageARB.StaticDraw);
        }
    }

    public void Bind() => gl.BindBuffer(bufferType, handle);

    public void Dispose() => gl.DeleteBuffer(handle);
    public void UnBind() => gl.BindBuffer(bufferType, 0);
}
