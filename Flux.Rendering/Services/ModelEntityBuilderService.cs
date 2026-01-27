using System.Numerics;
using DefaultEcs;
using Flux.Ecs;
using Flux.MathAddon;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.GLPrimitives.Textures;
using Flux.Rendering.ResourceManagers;
using Flux.Resources;
using Prowl.Slang;
using Silk.NET.OpenGL;
using EntryPoint = Flux.Slang.EntryPoint;
using Shader = Flux.Rendering.GLPrimitives.Shader;
using Texture = Flux.Rendering.GLPrimitives.Textures.Texture;

namespace Flux.Rendering.Services;

public class ModelEntityBuilderService
{
    readonly LoadingService loadingService;
    readonly ResourcesRepository resourcesRepository;
    readonly World world;

    string name = "Object";
    FileInfo shaderFile = null!;
    EntryPoint[] shaderEntryPoints = null!;
    FileInfo? model;
    Mesh<Vertex>? mesh;
    readonly Dictionary<uint, FileInfo> textures = [];

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

    public ModelEntityBuilderService Shader(FileInfo file, EntryPoint[] entryPoints)
    {
        shaderEntryPoints = entryPoints;
        shaderFile = file;
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
    public ModelEntityBuilderService Texture(uint binding, FileInfo file)
    {
        textures[binding] = file;
        return this;
    }
    public ModelEntityBuilderService ClearTextures()
    {
        textures.Clear();
        return this;
    }
    public ModelEntityBuilderService RemoveTexture(uint binding)
    {
        textures.Remove(binding);
        return this;
    }

    public Entity Create()
    {
        var entity = world.CreateEntity();

        entity.Set(Guid.NewGuid());
        entity.Set(name);
        entity.Set(transform);
        entity.Set(resourcesRepository);

        var textureSetting = new TextureSetting
        {
            WrapModeS = TextureWrapMode.ClampToBorder,
            WrapModeT = TextureWrapMode.ClampToBorder,
            TextureMinFilter = TextureMinFilter.Linear,
            TextureMagFilter = TextureMagFilter.Linear,
            Mipmap = new MipmapSetting.NoMipmap()
        };

        var registeredTexture = textures
            .Select(t => (binding : t.Key, texture : resourcesRepository.Register<Texture, TextureCreationInfo>(new TextureCreationInfo(t.Value, textureSetting))))
            .ToArray();

        entity.AddResource(registeredTexture.Select(t => t.texture).ToArray());
        
        var shaderId = resourcesRepository.Register<Shader, ShaderCreationInfo>(new ShaderCreationInfo(shaderFile, shaderEntryPoints));
        entity.AddResource(shaderId);
        
        var materialId = resourcesRepository.Register<Material, MaterialCreationInfo>(new MaterialCreationInfo(shaderId, registeredTexture));
        entity.AddResource(materialId);
        
        Model? entityModel = null;

        if (model is not null)
            entityModel = loadingService.LoadModel(model, materialId, resourcesRepository);
        if (mesh is not null)
            entityModel = new Model([mesh.Value], materialId, resourcesRepository);

        if (entityModel is null)
            throw new InvalidOperationException("Model or Mesh must be set before creating an entity");

        entity.Set(entityModel.Value);

        return entity;
    }
}