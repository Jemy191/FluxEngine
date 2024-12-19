using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct Mesh : IBindable, IDisposable
{
    public const uint VertexSize = 14;
    readonly GL gl;

    readonly VertexArrayObject<float> vao;
    readonly BufferObject<float> vbo;
    readonly BufferObject<uint> ebo;

    readonly uint indicesCount;

    public Mesh(GL gl, float[] vertices, uint[] indices, bool useColor = false)
    {
        this.gl = gl;

        indicesCount = (uint)indices.Length;

        vao = new VertexArrayObject<float>(gl);
        using (vao.ScopeBind())
        {
            ebo = new BufferObject<uint>(gl, BufferTargetARB.ElementArrayBuffer);
            ebo.Bind();
            ebo.SendData(indices);

            vbo = new BufferObject<float>(gl, BufferTargetARB.ArrayBuffer);
            vbo.Bind();
            vbo.SendData(vertices);

            var vertexSize = VertexSize;
            if (useColor)
                vertexSize += 3;
            vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, vertexSize, 0);
            vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, vertexSize, 3);
            vao.VertexAttributePointer(2, 3, VertexAttribPointerType.Float, vertexSize, 6);
            vao.VertexAttributePointer(3, 3, VertexAttribPointerType.Float, vertexSize, 9);
            vao.VertexAttributePointer(4, 2, VertexAttribPointerType.Float, vertexSize, 12);
            if (useColor)
                vao.VertexAttributePointer(5, 3, VertexAttribPointerType.Float, vertexSize, 14);
        }
        vbo.Unbind();
        ebo.Unbind();
    }

    internal unsafe void Draw() => gl.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, null);

    public void Bind() => vao.Bind();
    public void Unbind() => vao.Unbind();

    public void Dispose()
    {
        vao.Dispose();
        vbo.Dispose();
        ebo.Dispose();
    }
}