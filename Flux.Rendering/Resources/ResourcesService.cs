using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flux.Rendering.Resources;

public class ResourcesService : IDisposable
{
    readonly GL gl;
    readonly ModelLoaderService modelLoaderService;
    readonly List<IDisposable> resources = new();


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
    public Texture LoadTexture(Path path)
    {
        using var image = Image.Load<Rgba32>(ToAssetPath(path));

        var texture = new Texture(gl, image);
        resources.Add(texture);
        return texture;
    }

    public Model LoadModel(Path path, Material material)
    {
        var meshes = modelLoaderService.LoadMeshes(ToAssetPath(path));
        resources.AddRange(meshes.Cast<IDisposable>());
        return new(meshes, material);
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