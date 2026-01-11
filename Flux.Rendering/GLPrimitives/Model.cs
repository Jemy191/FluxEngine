using Flux.Abstraction;
using Flux.Resources;

namespace Flux.Rendering.GLPrimitives;

public readonly struct Model : IResource , IDisposable
{
    readonly Mesh<Vertex>[] meshes;
    readonly Material material;

    public Model(Mesh<Vertex>[] meshes, ResourceId<Material> materialId, ResourcesRepository resourcesRepository)
    {
        this.meshes = meshes;
        material = resourcesRepository.Get(materialId);
    }

    public readonly void Draw(IEnumerable<Uniform> uniforms)
    {
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
        material.Dispose();
        foreach (var mesh in meshes)
        {
            mesh.Dispose();
        }
    }
}
