using Silk.NET.OpenGL;

namespace Flux.Rendering.GLPrimitives;

public readonly struct UniformBufferObject<TDataType> : IBindable, IDisposable
    where TDataType : unmanaged
{
    readonly uint bindingPoint;
    readonly BufferObject<TDataType> ubo;

    public UniformBufferObject(GL gl, uint bindingPoint)
    {
        this.bindingPoint = bindingPoint;
        ubo = new BufferObject<TDataType>(gl, BufferTargetARB.UniformBuffer);
        ubo.PreAllocate();
    }

    public void SendData(TDataType data)
    {
        ubo.Bind();
        ubo.SendData(data);
    }

    /// <summary> The bind is OpenGL global, so it only needs to be called once. </summary>
    public void Bind() => ubo.BindBase(bindingPoint);
    public void Unbind() => ubo.UnbindBase(bindingPoint);
    public void Dispose() => ubo.Dispose();
}