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
    readonly SlangCompiler slangCompiler;

    public LoadingService(GL gl, ModelLoaderService modelLoaderService, SlangCompiler slangCompiler)
    {
        this.gl = gl;
        this.modelLoaderService = modelLoaderService;
        this.slangCompiler = slangCompiler;
    }

    public Shader LoadShader(FileInfo slangFile) => slangCompiler
        .Compile(slangFile)
        .Match(
            success => new Shader(gl, success.VertexSource, success.FragmentSource),
            fail => throw fail.DiagnosticInfo.GetException() ?? throw new Exception($"Compilation of {slangFile.FullName} fail but there is no diagnostic message.")
        );

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