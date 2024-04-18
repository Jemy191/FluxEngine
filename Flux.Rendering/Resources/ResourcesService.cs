using Flux.Engine;
using Flux.Engine.Assets;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flux.Rendering.Resources;

public class ResourcesService : IDisposable
{
    readonly GL gl;
    readonly ModelLoaderService modelLoaderService;
    readonly List<IDisposable> resources = new List<IDisposable>();


    public ResourcesService(GL gl, ModelLoaderService modelLoaderService)
    {
        this.gl = gl;
        this.modelLoaderService = modelLoaderService;
    }

    public Shader LoadShader(Path vertexPath, Path fragmentPath)
    {
        var shader = new Shader(gl, LoadAssetFile(vertexPath), LoadAssetFile(fragmentPath));
        resources.Add(shader);
        return shader;
    }
    public Shader LoadShader(IReadOnlyDictionary<ShaderType, ShaderAsset> shaderAssets)
    {
        var shader = new Shader(gl, shaderAssets[ShaderType.Vertex].Code, shaderAssets[ShaderType.Fragment].Code);
        resources.Add(shader);
        return shader;
    }
    public Texture LoadTexture(Path path)
    {
        using var image = Image.Load<Rgba32>(ToAssetPath(path));

        var texture = new Texture(gl, image);
        resources.Add(texture);
        return texture;
    }

    public Texture LoadTexture(TextureAsset textureAsset)
    {
        var size = textureAsset.Size;
        var texture = new Texture(gl, textureAsset.Pixels.AsSpan(), size.X, size.Y);
        resources.Add(texture);
        return texture;
    }

    public Model LoadModel(Path path, Material material)
    {
        var meshes = modelLoaderService.LoadMeshes(ToAssetPath(path));
        resources.AddRange(meshes.Cast<IDisposable>());
        return new Model(meshes, material);
    }
    public Model LoadModel(MeshAsset meshAsset, Material material)
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
        resources.Add(mesh);
        return new Model([mesh], material);
    }

    static string ToAssetPath(Path path) => System.IO.Path.Combine("Assets", path);
    static string LoadAssetFile(Path path) => File.ReadAllText(ToAssetPath(path));

    public void Dispose()
    {
        foreach (var resource in resources)
        {
            resource.Dispose();
        }
    }
}