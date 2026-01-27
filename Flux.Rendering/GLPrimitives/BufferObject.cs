using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives;

public readonly struct BufferObject<TDataType> : IDisposable
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

    public unsafe void SendData(TDataType data)
    {
        var size = (nuint)sizeof(TDataType);
        gl.BufferData(bufferType, size, &data, BufferUsageARB.StaticDraw);
    }

    public unsafe void PreAllocate(uint length = 1)
    {
        var size = int.Max(sizeof(TDataType), 16);
        var fullSize = (nuint)(length * size);
        gl.BufferData(bufferType, fullSize, null, BufferUsageARB.StaticDraw);
    }

    public void Bind() => gl.BindBuffer(bufferType, handle);
    public void BindBase(uint index) => gl.BindBufferBase(bufferType, index, handle);
    public void Unbind() => gl.BindBuffer(bufferType, 0);
    public void UnbindBase(uint index) => gl.BindBufferBase(bufferType, index, 0);
    public void Dispose() => gl.DeleteBuffer(handle);
}