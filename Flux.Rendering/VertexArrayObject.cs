using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct VertexArrayObject<TVertexType> : IBindable, IDisposable
    where TVertexType : unmanaged
{
    readonly uint handle;
    readonly GL gl;

    public VertexArrayObject(GL gl)
    {
        this.gl = gl;

        handle = this.gl.GenVertexArray();
    }

    public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize, int offSet)
    {
        var stride = vertexSize * (uint)sizeof(TVertexType);
        var pointer = (void*)(offSet * sizeof(TVertexType));
        gl.VertexAttribPointer(index, count, type, false, stride, pointer);
        gl.EnableVertexAttribArray(index);
    }

    public void Bind() => gl.BindVertexArray(handle);
    public void Unbind() => gl.BindVertexArray(0);
    public void Dispose() => gl.DeleteVertexArray(handle);
}
