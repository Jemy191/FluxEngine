using System.Numerics;
using DefaultEcs;
using Flux.Ecs;
using Flux.MathAddon;
using Flux.Rendering.GLPrimitives;
using Flux.Resources;

namespace Flux.Rendering.Services;

public class ModelEntityBuilderService
{
    readonly LoadingService loadingService;
    readonly ResourcesRepository resourcesRepository;
    readonly World world;

    string name = "Object";
    FileInfo vertex = null!;
    FileInfo fragment = null!;
    FileInfo? model;
    Mesh<Vertex>? mesh;
    readonly Dictionary<string, FileInfo> textures = [];
    readonly List<Uniform> uniforms = [];

    Transform transform = new Transform();

    public ModelEntityBuilderService(IEcsWorldService ecsService, LoadingService loadingService, ResourcesRepository resourcesRepository)
    {
        this.loadingService = loadingService;
        this.resourcesRepository = resourcesRepository;
        world = ecsService.World;
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
    public ModelEntityBuilderService Model(FileInfo file)
    {
        mesh = null;
        model = file;
        return this;
    }
    public ModelEntityBuilderService Mesh(Mesh<Vertex> mesh)
    {
        model = null;
        this.mesh = mesh;
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
        var toRemove = uniforms.Single(u => string.Equals(u.name, name, StringComparison.Ordinal));
        uniforms.Remove(toRemove);

        return this;
    }

    public Entity Create()
    {
        var entity = world.CreateEntity();

        entity.Set(Guid.NewGuid());
        entity.Set(name);
        entity.Set(transform);
        entity.Set(resourcesRepository);

        var registeredTexture = textures
            .Select(texture => (uniformName: texture.Key, texture: resourcesRepository.Register<Texture>(texture.Value)))
            .ToArray();

        entity.AddResource(registeredTexture.Select(t => t.texture).ToArray());
        
        var shaderId = resourcesRepository.Register<Shader>((vertex, fragment));
        entity.AddResource(shaderId);
        
        var materialId = resourcesRepository.Register<Material>((shaderId, registeredTexture.ToArray(), uniforms.ToArray()));
        entity.AddResource(materialId);
        
        Model? entityModel = null;

        if (model is not null)
            entityModel = loadingService.LoadModel(model, materialId, resourcesRepository);
        if (mesh is not null)
            entityModel = new Model([mesh.Value], materialId, resourcesRepository);

        if (entityModel is null)
            throw new InvalidOperationException("Model or Mesh must be set before creating entity");

        entity.Set(entityModel.Value);

        return entity;
    }
}