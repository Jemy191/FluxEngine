using System.Numerics;
using DefaultEcs;
using Flux.Ecs;
using Flux.Engine;
using Flux.Engine.Assets;
using Flux.MathAddon;

namespace Flux.Rendering;

public class ModelEntityBuilderService
{
    readonly World world;
    readonly GL gl;

    string name = "Object";
    MeshAsset? meshAsset;
    readonly Dictionary<string, TextureAsset> textureAssets = []; 
    ShaderAsset? shaderAsset;
    readonly List<Uniform> uniforms = [];

    Transform transform = new Transform();

    public ModelEntityBuilderService(IEcsWorldService ecsService, GL gl)
    {
        world = ecsService.World;
        this.gl = gl;
    }

    public ModelEntityBuilderService Name(string name)
    {
        this.name = name;
        return this;
    }

    public ModelEntityBuilderService Shader(ShaderAsset asset)
    {
        shaderAsset = asset;
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
    public ModelEntityBuilderService Texture(string name, TextureAsset asset)
    {
        textureAssets[name] = asset;
        return this;
    }
    public ModelEntityBuilderService RemoveTexture(string name)
    {
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
        if (meshAsset is null || shaderAsset is null)
            throw new Exception("Unable to create model entity");
        
        var shader = new Shader(gl, shaderAsset.StageCodes);
        var textureArray = textureAssets.Select(texture => (texture.Key, CreateTexture(texture.Value))).ToArray();
        var material = new Material(shader, textureArray, uniforms.ToArray());
        var model = CreateModel(meshAsset, material);

        var entity = world.CreateEntity();
        entity.Set(name);
        entity.Set(transform);
        entity.Set(model);

        return entity;
    }

    Texture CreateTexture(TextureAsset textureAsset)
    {
        var size = textureAsset.Size;
        var texture = new Texture(gl, textureAsset.Pixels.AsSpan(), size.X, size.Y);
        return texture;
    }
    
    Model CreateModel(MeshAsset meshAsset, Material material)
    {
        var vertices = meshAsset.Vertices
            .SelectMany(v =>
                new[]
                {
                    v.Position.X, v.Position.Y, v.Position.Z,
                    v.Normal.X, v.Normal.Y, v.Normal.Z,
                    v.Tangent.X, v.Tangent.Y, v.Tangent.Z,
                    v.Bitangent.X, v.Bitangent.Y, v.Bitangent.Z,
                    v.TexCoords.X, v.TexCoords.Y,
                    //v.Colors.X, v.Colors.Y, v.Colors.Z
                })
            .ToArray();

        var mesh = new Mesh(gl, vertices, meshAsset.Indices.ToArray());
        return new Model([mesh], material);
    }
}