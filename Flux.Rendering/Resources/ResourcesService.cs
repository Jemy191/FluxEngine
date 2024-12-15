using Flux.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flux.Rendering.Resources;

public class ResourcesService : IDisposable
{
    readonly GL gl;
    readonly ModelLoaderService modelLoaderService;
    readonly List<IDisposable> resources = [];
    
    public ResourcesService(GL gl, ModelLoaderService modelLoaderService)
    {
        this.gl = gl;
        this.modelLoaderService = modelLoaderService;
    }

    public Shader LoadShader(FileInfo vertexFile, FileInfo fragmentFile)
    {
        var shader = new Shader(gl, LoadAssetFile(vertexFile), LoadAssetFile(fragmentFile));
        resources.Add(shader);
        return shader;
    }
    public Texture LoadTexture(FileInfo file)
    {
        using var image = Image.Load<Rgba32>(file.FullName);

        var texture = new Texture(gl, image);
        resources.Add(texture);
        return texture;
    }

    public Model LoadModel(FileInfo file, Material material)
    {
        var meshes = modelLoaderService.LoadMeshes(file);
        resources.AddRange(meshes.Cast<IDisposable>());
        return new(meshes, material);
    }

    public static string LoadAssetFile(FileInfo file) => File.ReadAllText(file.FullName);

    public void Dispose()
    {
        foreach (var resource in resources)
        {
            resource.Dispose();
        }
    }
}