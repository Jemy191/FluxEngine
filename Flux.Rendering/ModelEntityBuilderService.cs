using System.Numerics;
using DefaultEcs;
using Flux.Ecs;
using Flux.MathAddon;
using Flux.Rendering.Resources;

namespace Flux.Rendering;

public class ModelEntityBuilderService
{
    readonly World world;
    readonly ResourcesService resourcesService;

    string name = "Object";
    FileInfo vertex;
    FileInfo fragment;
    FileInfo mesh;
    readonly Dictionary<string, FileInfo> textures = new();
    readonly List<Uniform> uniforms = new();

    Transform transform = new();

    public ModelEntityBuilderService(IEcsWorldService ecsService, ResourcesService resourcesService)
    {
        world = ecsService.World;
        this.resourcesService = resourcesService;
    }

    public ModelEntityBuilderService Name(string name)
    {
        this.name = name;
        return this;
    }

    public ModelEntityBuilderService Vertex(FileInfo file)
    {
        vertex = file;
        return this;
    }
    public ModelEntityBuilderService Fragment(FileInfo file)
    {
        fragment = file;
        return this;
    }
    public ModelEntityBuilderService Mesh(FileInfo file)
    {
        mesh = file;
        return this;
    }
    public ModelEntityBuilderService Transform(Transform transform)
    {
        this.transform = transform;
        return this;
    }
    public ModelEntityBuilderService Position(Vector3 position)
    {
        transform.Position = position;
        return this;
    }
    public ModelEntityBuilderService Rotation(Quaternion rotation)
    {
        transform.Rotation = rotation;
        return this;
    }
    public ModelEntityBuilderService Scale(Vector3 scale)
    {
        transform.Scale = scale;
        return this;
    }
    public ModelEntityBuilderService Texture(string name, FileInfo file)
    {
        textures[name] = file;
        return this;
    }
    public ModelEntityBuilderService ClearTextures()
    {
        textures.Clear();
        return this;
    }
    public ModelEntityBuilderService RemoveTexture(string name)
    {
        textures.Remove(name);
        return this;
    }
    public ModelEntityBuilderService AddUniform(Uniform uniform)
    {
        uniforms.Add(uniform);
        return this;
    }
    public ModelEntityBuilderService AddUniform<T>(string name, T value)
    {
        uniforms.Add(new Uniform<T>(name, value));
        return this;
    }
    public ModelEntityBuilderService ClearUniforms()
    {
        uniforms.Clear();
        return this;
    }
    public ModelEntityBuilderService RemoveUniform(string name)
    {
        var toRemvoe = uniforms.Single(u => u.name == name);
        uniforms.Remove(toRemvoe);

        return this;
    }

    public Entity Create()
    {
        var shader = resourcesService.LoadShader(vertex, fragment);

        var textures = new List<(string uniformName, Texture texture)>();

        foreach (var texture in this.textures)
        {
            textures.Add((texture.Key, resourcesService.LoadTexture(texture.Value)));
        }

        var material = new Material(shader, textures.ToArray(), uniforms.ToArray());
        var model = resourcesService.LoadModel(mesh, material);

        var entity = world.CreateEntity();
        entity.Set(name);
        entity.Set(transform);
        entity.Set(model);

        return entity;
    }
}
