using System.Numerics;
using Flux.Rendering.DataStruct;
using Flux.Rendering.GLPrimitives;

namespace Flux.Rendering;

public class ModelViewProjectionBuffer : IBindable, IDisposable
{
    readonly UniformBufferObject<ViewProjection> vp;
    readonly UniformBufferObject<Matrix4x4> model;

    /// <summary>
    /// Bind by default to ubo buffer 0 and 1
    /// </summary>
    public ModelViewProjectionBuffer(GL gl, uint viewProjectionBindingPoint = 0, uint modelBindingPoint = 1)
    {
        vp = new UniformBufferObject<ViewProjection>(gl, viewProjectionBindingPoint);
        model = new UniformBufferObject<Matrix4x4>(gl, modelBindingPoint);
    }

    public void SetViewProjection(ViewProjection vp) => this.vp.SendData(vp);
    public void SetModel(Matrix4x4 model) => this.model.SendData(model);

    public void Bind()
    {
        vp.Bind();
        model.Bind();
    }
    public void Unbind()
    {
        vp.Unbind();
        model.Unbind();
    }
    public void Dispose()
    {
        vp.Dispose();
        model.Dispose();
    }
}