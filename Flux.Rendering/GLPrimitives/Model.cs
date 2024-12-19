namespace Flux.Rendering.GLPrimitives;

public readonly struct Model : IDisposable
{
    readonly Mesh[] meshes;
    readonly Material material;

    public Model(Mesh[] meshes, Material material)
    {
        this.meshes = meshes;
        this.material = material;
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
