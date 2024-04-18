using System.Numerics;
using DefaultEcs;
using Flux.Ecs;
using Flux.Engine;
using Flux.Engine.Assets;
using Flux.MathAddon;
using Flux.Rendering.Resources;

namespace Flux.Rendering;

public class ModelEntityBuilderService
{
    readonly World world;
    readonly ResourcesService resourcesService;

    string name = "Object";
    Path vertex;
    Path fragment;
    Path mesh;
    MeshAsset? meshAsset;
    readonly Dictionary<string, TextureAsset> textureAssets = [];
    readonly Dictionary<ShaderType, ShaderAsset> shaderAssets = [];
    readonly Dictionary<string, Path> textures = [];
    readonly List<Uniform> uniforms = [];

    Transform transform = new Transform();

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

    public ModelEntityBuilderService Shader(ShaderAsset asset)
    {
        shaderAssets[asset.Type] = asset;
        return this;
    }
    public ModelEntityBuilderService Vertex(Path path)
    {
        vertex = path;
        return this;
    }
    public ModelEntityBuilderService Fragment(Path path)
    {
        fragment = path;
        return this;
    }
    public ModelEntityBuilderService Mesh(Path path)
    {
        mesh = path;
        return this;
    }
    public ModelEntityBuilderService Mesh(MeshAsset asset)
    {
        meshAsset = asset;
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
    public ModelEntityBuilderService Texture(string name, string path)
    {
        textures[name] = path;
        return this;
    }
    public ModelEntityBuilderService Texture(string name, TextureAsset asset)
    {
        textureAssets[name] = asset;
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
        textureAssets.Remove(name);
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
        var toRemove = uniforms.Single(u => u.name == name);
        uniforms.Remove(toRemove);

        return this;
    }

    public Entity Create()
    {
        Shader shader;
        if(shaderAssets.Count > 0)
            shader = resourcesService.LoadShader(shaderAssets);
        else
            shader = resourcesService.LoadShader(vertex, fragment);

        (string name, Texture texture)[] textureArray;
        if(textureAssets.Count > 0)
            textureArray = textureAssets.Select(texture => (texture.Key, resourcesService.LoadTexture(texture.Value))).ToArray();
        else
            textureArray = textures.Select(texture => (texture.Key, resourcesService.LoadTexture(texture.Value))).ToArray();
        
        var material = new Material(shader, textureArray, uniforms.ToArray());

        Model model;
        if (meshAsset is not null)
            model = resourcesService.LoadModel(meshAsset, material);
        else
            model = resourcesService.LoadModel(mesh, material);

        var entity = world.CreateEntity();
        entity.Set(name);
        entity.Set(transform);
        entity.Set(model);

        return entity;
    }
}