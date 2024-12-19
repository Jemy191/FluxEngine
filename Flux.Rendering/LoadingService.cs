using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flux.Rendering;

public class LoadingService
{
    readonly GL gl;
    readonly ModelLoaderService modelLoaderService;
    
    public LoadingService(GL gl, ModelLoaderService modelLoaderService)
    {
        this.gl = gl;
        this.modelLoaderService = modelLoaderService;
    }

    public Shader LoadShader(FileInfo vertexFile, FileInfo fragmentFile)
    {
        var shader = new Shader(gl, LoadAssetFile(vertexFile), LoadAssetFile(fragmentFile));
        return shader;
    }
    public Texture LoadTexture(FileInfo file)
    {
        using var image = Image.Load<Rgba32>(file.FullName);

        var texture = new Texture(gl, image);
        return texture;
    }

    public Model LoadModel(FileInfo file, Material material)
    {
        var meshes = modelLoaderService.LoadMeshes(file);
        return new Model(meshes, material);
    }

    public static string LoadAssetFile(FileInfo file) => File.ReadAllText(file.FullName);
}