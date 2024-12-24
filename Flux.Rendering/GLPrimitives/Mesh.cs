using Flux.Rendering.Extensions;
using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives;

public readonly struct Mesh : IBindable, IDisposable
{
    readonly GL gl;

    readonly VertexArrayObject<Vertex> vao;
    readonly BufferObject<Vertex> vbo;
    readonly BufferObject<uint> ebo;

    readonly uint indicesCount;

    public Mesh(GL gl, ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices)
    {
        this.gl = gl;

        indicesCount = (uint)indices.Length;

        vao = new VertexArrayObject<Vertex>(gl);
        using (vao.ScopeBind())
        {
            ebo = new BufferObject<uint>(gl, BufferTargetARB.ElementArrayBuffer);
            ebo.Bind();
            ebo.SendData(indices);

            vbo = new BufferObject<Vertex>(gl, BufferTargetARB.ArrayBuffer);
            vbo.Bind();
            vbo.SendData(vertices);
            
            foreach (var vertexAttribute in Vertex.GetVertexAttributesLayout())
            {
                vao.VertexAttributePointer(vertexAttribute);
            }
        }
        vbo.Unbind();
        ebo.Unbind();
    }

    public unsafe void Draw() => gl.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, null);

    public void Bind() => vao.Bind();
    public void Unbind() => vao.Unbind();

    public void Dispose()
    {
        vao.Dispose();
        vbo.Dispose();
        ebo.Dispose();
    }
}