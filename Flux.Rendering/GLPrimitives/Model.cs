using Flux.Abstraction;
using Flux.Resources;

namespace Flux.Rendering.GLPrimitives;

public readonly struct Model : IResource , IDisposable
{
    readonly Mesh[] meshes;
    readonly Material material;

    public Model(Mesh[] meshes, Resource<Material> materialId, ResourcesRepository resourcesRepository)
    {
        this.meshes = meshes;
        material = resourcesRepository.GetResource(materialId);
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
