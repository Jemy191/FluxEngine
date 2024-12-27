using Flux.Rendering.Extensions;
using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives;

public readonly struct Mesh<T> : IBindable, IDisposable where T : unmanaged, IVertexLayout
{
    readonly GL gl;

    readonly VertexArrayObject<T> vao;
    readonly BufferObject<T> vbo;
    readonly BufferObject<uint> ebo;

    readonly uint indicesCount;

    public Mesh(GL gl, ReadOnlySpan<T> vertices, ReadOnlySpan<uint> indices)
    {
        this.gl = gl;

        indicesCount = (uint)indices.Length;

        vao = new VertexArrayObject<T>(gl);
        using (vao.ScopeBind())
        {
            ebo = new BufferObject<uint>(gl, BufferTargetARB.ElementArrayBuffer);
            ebo.Bind();
            ebo.SendData(indices);

            vbo = new BufferObject<T>(gl, BufferTargetARB.ArrayBuffer);
            vbo.Bind();
            vbo.SendData(vertices);

            var t = T.GetVertexAttributesLayout();
            foreach (var vertexAttribute in t)
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