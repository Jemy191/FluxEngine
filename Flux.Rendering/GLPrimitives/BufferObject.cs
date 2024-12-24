using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives;

public readonly struct BufferObject<TDataType> : IBindable, IDisposable
    where TDataType : unmanaged
{
    readonly uint handle;
    readonly BufferTargetARB bufferType;
    readonly GL gl;

    public BufferObject(GL gl, BufferTargetARB bufferType)
    {
        this.gl = gl;
        this.bufferType = bufferType;

        handle = this.gl.GenBuffer();
    }
    
    public unsafe void SendData(ReadOnlySpan<TDataType> data)
    {
        fixed (void* d = data)
        {
            var size = (nuint)(data.Length * sizeof(TDataType));
            gl.BufferData(bufferType, size, d, BufferUsageARB.StaticDraw);
        }
    }

    public void Bind() => gl.BindBuffer(bufferType, handle);
    public void Unbind() => gl.BindBuffer(bufferType, 0);
    public void Dispose() => gl.DeleteBuffer(handle);
}
