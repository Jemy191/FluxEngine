namespace Flux.Rendering.GLPrimitives;

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

    public void VertexAttributePointer(VertexAttributeData attributeData) => VertexAttributePointer(attributeData.Index, attributeData.Count, attributeData.Type, attributeData.OffSet, attributeData.ConsecutiveElementCount);
    public unsafe void VertexAttributePointer(uint index, int count, VertexAttributePointerType type, uint offSet, uint consecutiveElementCount = 1)
    {
        ArgumentOutOfRangeException.ThrowIfZero(consecutiveElementCount);

        var stride = (uint)sizeof(TVertexType) * consecutiveElementCount;
        var pointer = (void*)offSet;

        var gl1 = gl;
        type.Match(
            f => gl1.VertexAttribPointer(index, count, f.Type, false, stride, pointer),
            i => gl1.VertexAttribIPointer(index, count, i.Type, stride, pointer));
        
        gl.EnableVertexAttribArray(index);
    }

    public void Bind() => gl.BindVertexArray(handle);
    public void Unbind() => gl.BindVertexArray(0);
    public void Dispose() => gl.DeleteVertexArray(handle);
}