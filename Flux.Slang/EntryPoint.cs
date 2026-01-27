using Prowl.Slang;

namespace Flux.Slang;

public readonly record struct EntryPoint(ShaderStage Stage, string Name)
{
    public static readonly EntryPoint CommonVertex = new EntryPoint(ShaderStage.Vertex, "VertexMain");
    public static readonly EntryPoint CommonFragment = new EntryPoint(ShaderStage.Fragment, "FragmentMain");

    public static readonly EntryPoint[] CommonVertexFrag =
    [
        CommonVertex,
        CommonFragment
    ];
}