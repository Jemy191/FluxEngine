using Flux.Abstraction;
using Flux.Resources;
using Flux.Resources.ResourceHandles;

namespace Flux.Rendering.GLPrimitives;

public readonly struct Model : IResource
{
    readonly Mesh<Vertex>[] meshes;
    readonly ResourceHandle<Material> materialHandle;

    public Model(Mesh<Vertex>[] meshes, ResourceId<Material> materialId, ResourcesRepository resourcesRepository)
    {
        this.meshes = meshes;
        materialHandle = resourcesRepository.Get(materialId);
    }

    public void Draw(IEnumerable<Uniform> uniforms)
    {
        var material = materialHandle.Resource;

        material.Use();
        material.SetUniforms(uniforms);

        foreach (var mesh in meshes)
        {
            mesh.Bind();
            mesh.Draw();
        }
    }

    public void Dispose()
    {
        materialHandle.Resource.Dispose();
        foreach (var mesh in meshes)
        {
            mesh.Dispose();
        }
    }
}