using Flux.Assets.Interfaces;
using Flux.Core;
using Flux.Slang;

namespace Flux.Rendering.Assets;

public class SlangLoader : IAssetLoader<ShaderAsset>
{
    readonly SlangCompiler slangCompiler;
    public HashSet<FileExtension> SupportedExtensions => [new FileExtension("slang")];

    public SlangLoader(SlangCompiler slangCompiler) => this.slangCompiler = slangCompiler;

    public ShaderAsset Load(FileInfo file)
    {
        slangCompiler.Compile(file, entryPoints)
            .Match(
                success => new ShaderAsset(success.VertexSource, success.FragmentSource),
                entryPointNotFound => throw new Exception($"Unable to find the {entryPointNotFound.EntryPoint.Stage} stage named {entryPointNotFound.EntryPoint.Name} entrypoint in {file.FullName}."),
                fail => throw fail.DiagnosticInfo.GetException() ?? throw new Exception($"Compilation of {file.FullName} fail but there is no diagnostic message.")
            );
    }
}