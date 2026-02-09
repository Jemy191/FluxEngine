using Flux.Assets;
using Flux.Rendering.Assets;
using Flux.Rendering.GLPrimitives;
using Flux.Rendering.GLPrimitives.Textures;
using Flux.Resources;
using Flux.Slang;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Flux.Rendering.Services;

public class LoadingService
{
    readonly GL gl;
    readonly ModelLoaderService modelLoaderService;

    public LoadingService(GL gl, ModelLoaderService modelLoaderService, SlangCompiler slangCompiler)
    {
        this.gl = gl;
        this.modelLoaderService = modelLoaderService;
    }


    public Texture LoadTexture(FileInfo file, TextureSetting textureSetting)
    {
        using var image = Image.Load<Rgba32>(file.FullName);
        return new Texture(gl, image, textureSetting);
    }

    public Model LoadModel(FileInfo file, ResourceId<Material> materialId, ResourcesRepository resourcesRepository)
    {
        var meshes = modelLoaderService.LoadMeshes(file);
        return new Model(meshes, materialId, resourcesRepository);
    }

    public static string LoadFile(FileInfo file) => File.ReadAllText(file.FullName);
}