using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct Mesh : IDisposable
{
    public const int VertexSize = 14;
    readonly GL gl;

    readonly VertexArrayObject<float, uint> vao;
    readonly BufferObject<float> vbo;
    readonly BufferObject<uint> ebo;

    readonly uint verticesCount;

    public Mesh(GL gl, float[] vertices, uint[] indices, bool useColor = false)
    {
        this.gl = gl;

        verticesCount = (uint)vertices.Length;
        ebo = new BufferObject<uint>(this.gl, indices, BufferTargetARB.ElementArrayBuffer);
        vbo = new BufferObject<float>(this.gl, vertices, BufferTargetARB.ArrayBuffer);
        vao = new VertexArrayObject<float, uint>(this.gl, vbo, ebo);

        uint vertexSize = VertexSize;
        if (useColor)
            vertexSize += 3;
        vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, vertexSize, 0);
        vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, vertexSize, 3);
        vao.VertexAttributePointer(2, 3, VertexAttribPointerType.Float, vertexSize, 6);
        vao.VertexAttributePointer(3, 3, VertexAttribPointerType.Float, vertexSize, 9);
        vao.VertexAttributePointer(4, 2, VertexAttribPointerType.Float, vertexSize, 12);
        if (useColor)
            vao.VertexAttributePointer(5, 3, VertexAttribPointerType.Float, vertexSize, 14);

        ebo.UnBind();
        vbo.UnBind();
        vao.UnBind();
    }

    public void Bind() => vao.Bind();
    internal void Draw() => gl.DrawArrays(PrimitiveType.Triangles, 0, verticesCount);

    public void Dispose()
    {
        vao.Dispose();
        vbo.Dispose();
        ebo.Dispose();
    }
}
