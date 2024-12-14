using Silk.NET.OpenGL;

namespace Flux.Rendering;

public readonly struct Mesh : IDisposable
{
    public const int VertexSize = 14;
    readonly GL Gl;

    readonly VertexArrayObject<float, uint> VAO;
    readonly BufferObject<float> VBO;
    readonly BufferObject<uint> EBO;

    readonly uint indicesCount;
    readonly uint verticesCount;

    public unsafe Mesh(GL gl, float[] vertices, uint[] indices, bool useColor = false)
    {
        Gl = gl;

        indicesCount = (uint)indices.Length;
        verticesCount = (uint)vertices.Length;
        EBO = new BufferObject<uint>(Gl, indices, BufferTargetARB.ElementArrayBuffer);
        VBO = new BufferObject<float>(Gl, vertices, BufferTargetARB.ArrayBuffer);
        VAO = new VertexArrayObject<float, uint>(Gl, VBO, EBO);

        uint vertexSize = VertexSize;
        if (useColor)
            vertexSize += 3;
        VAO.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, vertexSize, 0);
        VAO.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, vertexSize, 3);
        VAO.VertexAttributePointer(2, 3, VertexAttribPointerType.Float, vertexSize, 6);
        VAO.VertexAttributePointer(3, 3, VertexAttribPointerType.Float, vertexSize, 9);
        VAO.VertexAttributePointer(4, 2, VertexAttribPointerType.Float, vertexSize, 12);
        if (useColor)
            VAO.VertexAttributePointer(5, 3, VertexAttribPointerType.Float, vertexSize, 14);

        EBO.UnBind();
        VBO.UnBind();
        VAO.UnBind();
    }

    public void Bind() => VAO.Bind();
    internal unsafe void Draw() => //Gl.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, null);
    Gl.DrawArrays(PrimitiveType.Triangles, 0, verticesCount);

    public void Dispose()
    {
        VAO.Dispose();
        VBO.Dispose();
        EBO.Dispose();
    }
}
