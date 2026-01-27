using Flux.Slang;

namespace Flux.Rendering.ResourceManagers;

public readonly record struct ShaderCreationInfo(FileInfo ShaderFile, EntryPoint[] EntryPoints);